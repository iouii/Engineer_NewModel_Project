using System.Web;
using System.Web.Optimization;

namespace NewModelEx_S1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                          "~/Scripts/js/jquery-3.4.1.min.js",
                        "~/Scripts/js/bootstrap.bundle.min.js",
                        "~/Scripts/js/scripts.js",
                        "~/Scripts/js/Chart.min.js",
                       // "~/Scripts/Chart/chart-area-demo.js",
                       // "~/Scripts/Chart/chart-bar-demo.js",
                         "~/Scripts/Chart/datatables-demo.js",
                         "~/Scripts/js/jquery.dataTables.min.js",
                        "~/Scripts/js/dataTable.bootstrap4.min.js",
                //"~/Scripts/assete/chart-pie-demo.js",
                        "~/Scripts/assets/demo/datatables-demo.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/jquery-3.3.1.min.js",
                        "~/Scripts/gijgo.min.js",
                        "~/Scripts/js/all.min.js" 
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

                      
                     
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                     "~/Content/Site.css",
                     "~/Content/gijgo.min.css",
                      "~/Content/fontawesome-all.css",
                      "~/Content/styles.css",              
                       "~/Content/dataTables.bootstrap4.min.css"
                     
                     ));

            bundles.Add(new ScriptBundle("~/bundles/modernizrLogin").Include(
        "~/Scripts/modernizr-*",
        "~/Scripts/jquery-3.3.1.min.js",
        "~/Scripts/js/all.min.js"
        ));

            BundleTable.EnableOptimizations = true;
            
            /*ตัวนี้เป็นตัวเปิดใช้งาน ไฟล์ Config นะ พวก bootstrap css */
        }
    }
}
