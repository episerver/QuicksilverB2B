using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Commerce.Internal.Migration.Steps;
using Mediachase.Commerce.Shared;
using EPiServer.Enterprise;
using Mediachase.Commerce.Core;
using System.Web.Hosting;
using EPiServer.Core;
using System.IO;
using EPiServer.Logging;
using System.Text;
using System.Web;
using EPiServer.DataAbstraction;
using EPiServer.Scheduler;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    [ServiceConfiguration(typeof(IMigrationStep))]
    public class ImportMedia : IMigrationStep
    {
        private IProgressMessenger _progressMessenger;

        private Injected<IDataImporter> DataImporter { get; set; }
        public Injected<IScheduledJobExecutor> ScheduledJobExecutor { get; set; }
        public Injected<IScheduledJobRepository> ScheduledJobRepository { get; set; }

        public int Order
        {
            get { return 1200; }
        }

        public string Name { get { return "Quicksilver B2B Media"; } }
        public string Description { get { return "Import Media"; } }

        public bool Execute(IProgressMessenger progressMessenger)
        {
            _progressMessenger = progressMessenger;
            try
            {
                _progressMessenger.AddProgressMessageText("Importing assets...", false, 0);
                ImportAssets(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\B2BProductAssets.episerverdata"));
                ImportAssets(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\Hero.episerverdata"));
                _progressMessenger.AddProgressMessageText("Running EPiServer Find Content Indexing Job...", false, 0);
                var job = ScheduledJobRepository.Service.List().FirstOrDefault(x => x.Name.Equals("EPiServer Find Content Indexing Job"));
                if (job != null)
                {
                    var result = ScheduledJobExecutor.Service.StartAsync(job).Result;
                    if (result.Status == ScheduledJobExecutionStatus.Failed)
                    {
                        _progressMessenger.AddProgressMessageText("Quicksilver B2B Media failed: " + result.Message, true, 0);
                        LogManager.GetLogger(GetType()).Error(result.Message);
                    }
                }
                _progressMessenger.AddProgressMessageText("Done running EPiServer Find Content Indexing Job", false, 0);

                return true;
            }
            catch (Exception ex)
            {
                _progressMessenger.AddProgressMessageText("Quicksilver B2B Media failed failed: " + ex.Message + "Stack trace:" + ex.StackTrace, true, 0);
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