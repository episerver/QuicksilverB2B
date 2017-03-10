using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using EPiServer.Reference.Commerce.Site.Infrastructure;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Newtonsoft.Json;

namespace EPiServer.Reference.Commerce.Site
{
    public class Global : EPiServer.Global
    {

        /// </summary>
        static Global()
        {
            // TODO: Remove this when you are not going to use LocalDb anymore.
            ILogger log = LogManager.GetLogger();
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\appdata\");
            log.Debug("Setting data directory for Local DB to: " + dir.FullName);
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);
        }
        protected override void RegisterRoutes(RouteCollection routes)
        {
            base.RegisterRoutes(routes);

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { action = "Index", id = UrlParameter.Optional });

        }

        protected void Application_Start()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedRequiredAttribute), typeof(RequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedRegularExpressionAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedEmailAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedStringLengthAttribute), typeof(StringLengthAttributeAdapter));

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-1.11.1.js",
            });

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}