using System.Web;
using System.Web.Optimization;

namespace asi.asicentral.web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/MultiSelectedDatePicker").Include(
                  "~/Scripts/jquery.ui.core.js",
                  "~/Scripts/jquery.ui.datepicker.js",
                  "~/Scripts/jquery-ui.multidatespicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/datepicker.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/forms").Include(
                        "~/Content/form.css"));

            bundles.Add(new StyleBundle("~/Content/form").Include(
                        "~/Content/datepicker.css",
                        "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/MultiSelectedDatePicker").Include(
                    "~/Content/multiselecteddatepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                        "~/Scripts/jquery.dataTables*",
                        "~/Scripts/dataTables.*"));

            bundles.Add(new StyleBundle("~/dataTable/css").Include(
                        "~/Content/datatables-bootstrap.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}