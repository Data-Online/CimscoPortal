using System.Web;
using System.Web.Optimization;

namespace CimscoPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/slimscroll/jquery.slimscroll.min.js"
                ));
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-ui-bootstrap/ui-bootstrap.js",
               // "~/Scripts/charts/chartjs/tc-angular-chartjs.js",
                "~/Scripts/charts/flot/angular-flot.js",
                "~/Scripts/angular-ui-utils/angular-ui-utils.js",
                "~/Scripts/angular-sanitize.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/Content/css/bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css/beyond").Include(
                "~/Content/css/beyond.min.css",
                "~/Content/css/demo.min.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/typicons.min.css",
                "~/Content/css/weather-icons.min.css",
                "~/Content/css/animate.min.css",
                "~/Content/css/cimsco-custom.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/skin").Include(
                "~/Scripts/skins.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/beyond").Include(
                "~/Scripts/beyond.js"
                ));

            bundles.Add(new StyleBundle("~/css/bootstrap-rtl").Include(
                "~/Content/css/bootstrap-rtl.min.css"
                ));

            bundles.Add(new StyleBundle("~/css/beyond-rtl").Include(
                "~/Content/css/beyond-rtl.min.css",
                "~/Content/css/demo.min.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/typicons.min.css",
                "~/Content/css/weather-icons.min.css",
                "~/Content/css/animate.min.css"
                ));


        }
    }
}
