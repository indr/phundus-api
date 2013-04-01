using System.Web.Optimization;

namespace phiNdus.fundus.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bundling and minification is enabled or disabled by setting the value of the debug
            // attribute in the compilation Element in the Web.config file.
            // Enables bundling and minification and overrides any setting in the Web.config file.
            //BundleTable.EnableOptimizations = true;
        }
    }
}