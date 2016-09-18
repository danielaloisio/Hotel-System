using System.Web;
using System.Web.Optimization;

namespace KobraSoftware
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/sbadmin2").Include(
                       "~/Content/themes/sb-admin-2/bower-components/jquery/dist/jquery-min.js",
                       "~/Content/themes/sb-admin-2/bower-components/bootstrap/dist/js/bootstrap-min.js",
                       "~/Content/themes/sb-admin-2/bower-components/metisMenu/dist/metisMenu-min.js",
                       "~/Content/themes/sb-admin-2/bower-components/raphael/raphael-min.js",
                       "~/Content/themes/sb-admin-2/bower-components/datatables/media/js/jquery.dataTables-min.js",
                       "~/Content/themes/sb-admin-2/bower-components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap-min.js",
                       "~/Content/themes/sb-admin-2/dist/js/sb-admin-2.js",
                       "~/Scripts/jquery-meiomask-min.js"));
                      
            bundles.Add(new StyleBundle("~/Content/sbadmin2").Include(
                    "~/Content/themes/sb-admin-2/bower-components/bootstrap/dist/css/bootstrap-min.css",
                    "~/Content/themes/sb-admin-2/bower-components/metisMenu/dist/metisMenu-min.css",
                    "~/Content/themes/sb-admin-2/dist/css/timeline.css",
                    "~/Content/themes/sb-admin-2/bower-components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.css",
                    "~/Content/themes/sb-admin-2/bower-components/datatables-responsive/css/dataTables.responsive.css",
                    "~/Content/themes/sb-admin-2/dist/css/sb-admin-2.css",
                    "~/Content/themes/sb-admin-2/bower-components/fontawesome/css/font-awesome-min.css",
                    "~/Content/Custom.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}