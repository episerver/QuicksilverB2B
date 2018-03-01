using EPiServer.Commerce.Internal.Migration.Steps;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.Promotions;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;
using EPiServer.DataAccess;
using EPiServer.Enterprise;
using EPiServer.Enterprise.Transfer;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Mediachase.Commerce;
using Mediachase.Commerce.BackgroundTasks;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.ImportExport;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Extensions;
using Mediachase.Commerce.Shared;
using Mediachase.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    [ServiceConfiguration(typeof(IMigrationStep))]
    public class ImportSiteB2BContent : IMigrationStep
    {
        private IProgressMessenger _progressMessenger;

        private Injected<IDataImporter> DataImporter { get; set; }

        public int Order
        {
            get { return 1100; }
        }

        public string Name { get { return "Quicksilver B2B content"; } }
        public string Description { get { return "Import B2B catalog, assets Quicksilver B2B"; } }

        public bool Execute(IProgressMessenger progressMessenger)
        {
            _progressMessenger = progressMessenger;
            try
            {
                _progressMessenger.AddProgressMessageText("Importing product assets...", false, 0);
                ImportAssets(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\B2BProductAssets.episerverdata"));

                _progressMessenger.AddProgressMessageText("Importing catalog...", false, 0);
                ImportCatalog(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\CatalogExport_QuicksilverB2B_20161212-183053.zip"));

                _progressMessenger.AddProgressMessageText("Rebuilding index...", false, 0);
                BuildIndex(_progressMessenger, Mediachase.Commerce.Core.AppContext.Current.ApplicationName, true);
                _progressMessenger.AddProgressMessageText("Done rebuilding index", false, 0);

                return true;
            }
            catch (Exception ex)
            {
                _progressMessenger.AddProgressMessageText("ImportSiteB2BContent failed: " + ex.Message + "Stack trace:" + ex.StackTrace, true, 0);
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }
        }

        private void ImportAssets(string path)
        {
            var destinationRoot = ContentReference.GlobalBlockFolder;
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Clear the cache to ensure setup is running in a controlled environment, if perhaps we're developing and have just cleared the database.
            List<string> keys = new List<string>();
            foreach (DictionaryEntry entry in HttpRuntime.Cache)
            {
                keys.Add((string)entry.Key);
            }
            foreach (string key in keys)
            {
                HttpRuntime.Cache.Remove(key);
            }

            var options = new ImportOptions { KeepIdentity = true };

            var log = DataImporter.Service.Import(stream, destinationRoot, options);

            if (log.Errors.Any())
            {
                throw new Exception("Content could not be imported. " + GetStatus(log));
            }
        }

        private void ImportCatalog(string path)
        {
            var importJob = new ImportJob(path, "Catalog.xml", true);

            Action importCatalog = () =>
            {
                _progressMessenger.AddProgressMessageText("Importing Catalog content...", false, 20);
                Action<IBackgroundTaskMessage> addMessage = msg =>
                {
                    var isError = msg.MessageType == BackgroundTaskMessageType.Error;
                    var percent = (int)Math.Round(msg.GetOverallProgress() * 100);
                    var message = msg.Exception == null
                        ? msg.Message
                        : string.Format("{0} {1}", msg.Message, msg.ExceptionMessage);
                    _progressMessenger.AddProgressMessageText(message, isError, percent);
                };
                importJob.Execute(addMessage, CancellationToken.None);
                _progressMessenger.AddProgressMessageText("Done importing Catalog content", false, 60);
            };

            importCatalog();

            //We are running in front-end site context, the metafield update events are ignored, we need to sync manually
            _progressMessenger.AddProgressMessageText("Syncing metaclasses with content types", false, 60);
            SyncMetaClassesToContentTypeModels();
            _progressMessenger.AddProgressMessageText("Done syncing metaclasses with content types", false, 70);
        }

        private void BuildIndex(IProgressMessenger progressMessenger, string applicationName, bool rebuild)
        {
            var searchManager = new SearchManager(applicationName);
            searchManager.SearchIndexMessage += SearchManager_SearchIndexMessage;
            searchManager.BuildIndex(rebuild);
        }

        private void SearchManager_SearchIndexMessage(object source, SearchIndexEventArgs args)
        {
            // The whole index building process would take 20% (from 70 to 90) of the import process. Then the percent value here should be calculated based on 20%
            var percent = 70 + Convert.ToInt32(args.CompletedPercentage) * 2 / 10;
            _progressMessenger.AddProgressMessageText(args.Message, false, percent);
        }

        /// <summary>
        /// Synchronizes the meta classes to content type models.
        /// The synchronization will be done when site starts up. 
        /// To avoid restarting a site, we do the models synchronization manually.
        /// </summary>
        private static void SyncMetaClassesToContentTypeModels()
        {
            var cachedRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>() as ICachedRepository;
            if (cachedRepository != null)
            {
                cachedRepository.ClearCache();
            }

            cachedRepository = ServiceLocator.Current.GetInstance<IPropertyDefinitionRepository>() as ICachedRepository;
            if (cachedRepository != null)
            {
                cachedRepository.ClearCache();
            }

            var tasks = new List<Task>();

            var contentScanner = ServiceLocator.Current.GetInstance<IContentTypeModelScanner>();
            tasks.AddRange(contentScanner.RegisterModels());
            tasks.AddRange(contentScanner.Sync(Settings.Instance.EnableModelSyncCommit));

            Task.WaitAll(tasks.ToArray());
        }


        private string GetStatus(ITransferLog log)
        {
            var logMessage = new StringBuilder();
            var lineBreak = "<br>";

            if (log.Errors.Any())
            {
                foreach (string err in log.Errors)
                {
                    logMessage.Append(err).Append(lineBreak);
                }
            }

            if (log.Warnings.Any())
            {
                foreach (string err in log.Warnings)
                {
                    logMessage.Append(err).Append(lineBreak);
                }
            }
            return logMessage.ToString();
        }
    }
}