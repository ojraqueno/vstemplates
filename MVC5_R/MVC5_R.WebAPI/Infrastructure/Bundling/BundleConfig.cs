using System.Web.Optimization;

namespace MVC5_R.WebAPI.Infrastructure.Bundling
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            AddCoreBundles(bundles);
        }

        private static void AddCoreBundles(BundleCollection bundles)
        {
            bundles.AddStyleBundle("~/stylebundles/core",
                "~/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css",
                "~/wwwroot/styles/shared/app.css");

            bundles.AddScriptBundle("~/scriptbundles/core",
                "~/wwwroot/lib/jquery/dist/jquery.min.js",
                "~/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js");
        }
    }
}