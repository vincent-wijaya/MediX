using System.Web;
using System.Web.Optimization;

namespace MediX
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                    "~/Scripts/lib/jquery.min.js",
                    "~/Scripts/DataTables/jquery.dataTables.min.js",
                    "~/Scripts/DataTables/dataTables.bootstrap4.min.js",
                    "~/Scripts/DataTables/dataTables.buttons.min.js",
                    "~/Scripts/jszip.min.js",
                    "~/Scripts/pdfmake/pdfmake.min.js",
                    "~/Scripts/pdfmake/vfs_fonts.js",
                    "~/Scripts/DataTables/buttons.bootstrap4.min.js",
                    "~/Scripts/DataTables/buttons.flash.min.js",
                    "~/Scripts/DataTables/buttons.html5.min.js",
                    "~/Scripts/DataTables/buttons.print.min.js",
                    "~/Scripts/DataTables/buttons.jqueryui.min.js"

                ));
           
            bundles.Add(new ScriptBundle("~/bundles/mapbox").Include(
                    "~/Scripts/location.js"
                ));


        }
    }
}
