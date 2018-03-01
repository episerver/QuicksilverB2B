using System;
using System.Linq;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.Infrastructure.Business;
using EPiServer.ServiceLocation;
using EPiServer.ServiceLocation.Compatibility;
using Mediachase.Commerce.Engine.Events;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.InitializationModules
{
    /// <summary>
    /// The initialization module for EPiServer Find Commerce.
    /// 
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(FindCommerceInitializationModule))]
    public class EPiServerFindCommerceInitializationModule : IConfigurableModule
    {
        //private static readonly ILogger Log = LogManager.GetLogger(typeof(FindCommerceInitializationModule));
        //private static CatalogKeyEventBroadcaster _broadcaster;
        //private static CatalogContentEventListener _listener;

        /// <summary>
        /// Initializes this instance.
        /// 
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// Gets called as part of the EPiServer Framework initialization sequence. Note that it will be called
        ///             only once per AppDomain, unless the method throws an exception. If an exception is thrown, the initialization
        ///             method will be called repeadetly for each request reaching the site until the method succeeds.
        /// 
        /// </remarks>
        public void Initialize(InitializationEngine context)
        {
            //CatalogContentClientConventions instance = context.Locate.Advanced.GetInstance<B2BCatalogContentClientConventions>();
            //try
            //{
            //    instance.ApplyConventions(SearchClient.Instance.Conventions);
            //}
            //catch (Exception ex)
            //{
            //    if (Log.IsErrorEnabled())
            //        Log.Error("Could not apply catalog content conventions.", ex);
            //}
            //_broadcaster = context.Locate.Advanced.GetInstance<CatalogKeyEventBroadcaster>();
            //_listener = context.Locate.Advanced.GetInstance<CatalogContentEventListener>();
            //_listener.AddEvent();
            //_broadcaster.InventoryUpdated += _listener.InventoryUpdated;
            //_broadcaster.PriceUpdated += _listener.PriceUpdated;
        }

        /// <summary>
        /// Preloads the specified parameters.
        /// 
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void Preload(string[] parameters)
        {
        }

        /// <summary>
        /// Resets the module into an uninitialized state.
        /// 
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// 
        /// <para>
        /// This method is usually not called when running under a web application since the web app may be shut down very
        ///             abruptly, but your module should still implement it properly since it will make integration and unit testing
        ///             much simpler.
        /// 
        /// </para>
        /// 
        /// <para>
        /// Any work done by <see cref="M:EPiServer.Framework.IInitializableModule.Initialize(EPiServer.Framework.Initialization.InitializationEngine)"/> as well as any code executing on <see cref="E:EPiServer.Framework.Initialization.InitializationEngine.InitComplete"/> should be reversed.
        /// 
        /// </para>
        /// 
        /// </remarks>
        public void Uninitialize(InitializationEngine context)
        {
            //_broadcaster.InventoryUpdated -= _listener.InventoryUpdated;
            //_broadcaster.PriceUpdated -= _listener.PriceUpdated;
            //_listener.RemoveEvent();
        }

        /// <inheritdoc/>
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            if (!context.StructureMap().GetAllInstances<IContentIndexer>().Any())
            {
                context.StructureMap().Configure(c => c.For<IContentIndexer>().Singleton().Use(ContentIndexer.Instance));
            }
            context.StructureMap().Configure(c => c.For<EventedIndexingSettings>().Singleton().Use(() => EventedIndexingSettings.Instance));
        }
    }
}