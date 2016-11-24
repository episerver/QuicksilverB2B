using System.Web.Optimization;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap*"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/js/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/b2b-js").Include(
                        "~/Scripts/b2b-js/OrderPads.js",
                        "~/Scripts/b2b-js/Budgeting.js",
                        "~/Scripts/b2b-js/QuickOrderInterface.js",
                        "~/Scripts/b2b-js/uploadFile.js",
                        "~/Scripts/b2b-js/Users.js",
                        "~/Scripts/b2b-js/Organization.js"));

            bundles.Add(new ScriptBundle("~/bundles/pickaday").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/pickaday.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include(
                "~/Scripts/jquery.easyautocomplete.min.js"));

            bundles.Add(new StyleBundle("~/styles/bundled").Include(
                        "~/Styles/style.css",
                        "~/Styles/b2b/global.css"
                ));
        }
    }
}