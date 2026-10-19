using System.Web;
using System.Web.Optimization;

namespace WebUI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

                   

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Scripts").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/menu.js", 
                        "~/Scripts/bootstrap-select.js",
                        "~/Scripts/script.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/JScript/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                   "~/Scripts/JScript/kendo.all.min.js",
                   "~/Scripts/JScript/kendo.aspnetmvc.min.js",
                   "~/Scripts/JScript/kendo.timezones.min.js"));
           

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/css/bootstrap.css",
                "~/Content/css/style.css",
                "~/Content/css/datepicker.css",
                "~/Content/css/bootstrap-select.css"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
                      "~/Content/themes/styles/kendo.common.min.css",
                      "~/Content/themes/styles/kendo.rtl.min.css",
                      "~/Content/themes/styles/kendo.default.min.css",
                      "~/Content/themes/styles/kendo.default.mobile.min.css"));

            BundleTable.EnableOptimizations = true;


        }
    }
}