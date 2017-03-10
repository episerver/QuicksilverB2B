using System;
using System.Linq;
using System.Reflection;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using log4net;

namespace EPiServer.Reference.Commerce.Manager
{
    [InitializableModule]
    // [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class InitializationModule : IConfigurableModule
    {
        // ReSharper disable once InconsistentNaming
        protected static ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Initialize(InitializationEngine context)
        {
            // Example to show that you can have your own initialization in Commerce Manager too
            // Very useful when you need to customize Commerce Manager or add your own service config
            _log.Debug("Initializing Commerce Manager.");

            // TODO: Remove this when you are not going to use LocalDb anymore.
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\appdata\");
            _log.Debug("Setting data directory for Local DB to: " + dir.FullName);
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);

        }

        public void Preload(string[] parameters) { }
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            //context.Container.Configure(c => c.For<ISomething>().Use<Something>());
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}