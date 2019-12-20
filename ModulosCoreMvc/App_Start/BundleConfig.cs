using System.Web.Optimization;

namespace Modulos_Core_MVC
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
                        "~/Scripts/modernizr-*"));

            //Para Layouts base
            bundles.Add(new ScriptBundle("~/bundles/bootstrapmaster").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-select.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/bootstrap-notify.min.js",
                       "~/Scripts/bootbox.min.js",
                      "~/Scripts/jqueryTmpl.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                "~/Scripts/bloodhound.min.js",
                "~/Scripts/typeahead.jquery.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstraptable").Include(
                     "~/Scripts/bootstrap-table.min.js",
                     "~/Scripts/bootstrap-table-es-SP.min.js",
                     "~/Scripts/tableExport.min.js",
                     "~/Scripts/bootstrap-table-export.min.js",
                     "~/Scripts/bootstrap-table-mobile.min.js",
                     "~/Scripts/bootstrap-table-multiple-search.min.js",
                      "~/Scripts/bootstrap-table-flat-json.min.js",
                     "~/Scripts/bootstrap-table-multiple-sort.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/validatorsform").Include(
                   "~/Scripts/smoke.min.js",
                   "~/Scripts/smoke-es.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstraplogin").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.min.js"));

            // STYLES
            bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/bootstrap.min.css",
                  "~/Content/bootstrap-select.min.css",
                  "~/Content/bootstrap-table.min.css",
                  "~/Content/animate.css",
                  "~/Content/smoke.min.css",
                  "~/Content/Site.css",
                  "~/Content/TimeLine.css",
                  "~/Content/checkbox.css",
                  "~/Content/seccionDiv.css",
                  "~/Content/uploadtextcontrol.css",
                  "~/Content/magicsuggest-min.css"));

            bundles.Add(new StyleBundle("~/Content/cssprint").Include(
                  "~/Content/bootstrap.min.css",
                  "~/Content/printpage.css",
                  "~/Content/paper.min.css"));

            bundles.Add(new StyleBundle("~/Content/csslogin").Include(
                  "~/Content/bootstrap.min.css",
                   "~/Content/animate.css",
                  "~/Content/login.css"));

        }
    }
}
