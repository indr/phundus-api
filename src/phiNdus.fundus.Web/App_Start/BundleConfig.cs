using System.Web.Optimization;

namespace phiNdus.fundus.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-resource.js",
                "~/Scripts/modules/project.js",
                "~/Scripts/modules/mongolab.js"));


            // Bundling and minification is enabled or disabled by setting the value of the debug
            // attribute in the compilation Element in the Web.config file.
            // Enables bundling and minification and overrides any setting in the Web.config file.
            //BundleTable.EnableOptimizations = true;
        }
    }
}