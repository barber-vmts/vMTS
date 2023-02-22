using System.Web;
using System.Web.Optimization;

namespace vMTS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUI", "https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jasnyCSS", "//cdnjs.cloudflare.com/ajax/libs/jasny-bootstrap/3.1.3/css/jasny-bootstrap.min.css").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/Moment").Include(
            "~/Scripts/moment.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/ionicons", "http://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundles/modernizr", "https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jasnyJS", "//cdnjs.cloudflare.com/ajax/libs/jasny-bootstrap/3.1.3/js/jasny-bootstrap.min.js").Include(
          "~/Scripts/bootstrap.js",
          "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/datepicker", "https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.4.0/js/bootstrap-datepicker.min.js").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/respond", "https://cdnjs.cloudflare.com/ajax/libs/respond.js/1.4.2/respond.min.js").Include(
                      "~/Scripts/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
          "~/Scripts/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/instructors").Include(
            "~/Scripts/instructors.js"));

            bundles.Add(new ScriptBundle("~/bundles/print").Include(
                        "~/Scripts/PrintRegistration.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_Index").Include(
          "~/Scripts/admin_Index.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_Registration").Include(
         "~/Scripts/admin_Registration.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_ExportCSV").Include(
          "~/Scripts/admin_ExportCSV.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_Class").Include(
          "~/Scripts/admin_ClassSchedule.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_Users").Include(
          "~/Scripts/admin_Users.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_Carousel").Include(
          "~/Scripts/admin_Carousel.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_Sponsors").Include(
          "~/Scripts/admin_Sponsors.js"));

            bundles.Add(new ScriptBundle("~/bundles/registration").Include(
         "~/Scripts/moment.js",
         "~/Scripts/registration.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin_PromoCodes").Include(
         "~/Scripts/admin_PromoCodes.js"));

            bundles.Add(new StyleBundle("~/boot-min/css", "//maxcdn.bootstrapcdn.com/bootswatch/3.3.4/simplex/bootstrap.min.css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/date-min/css", "https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.4.0/css/bootstrap-datepicker.min.css").Include(
                      "~/Content/bootstrap1.css"));

            bundles.Add(new StyleBundle("~/custom/css").Include(
          "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
