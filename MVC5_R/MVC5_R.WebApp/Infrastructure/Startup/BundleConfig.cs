using MVC5_R.WebApp.Infrastructure.Bundling;
using System.Web.Optimization;

namespace MVC5_R.WebApp.Infrastructure.Startup
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
                "~/wwwroot/lib/bootstrap/dist/css/bootstrap.css",
                "~/wwwroot/lib/font-awesome/css/font-awesome.min.css",
                "~/wwwroot/styles/shared/app.css");

            bundles.AddScriptBundle("~/scriptbundles/core",
                "~/wwwroot/lib/jquery/dist/jquery.min.js",
                "~/wwwroot/lib/jquery-validation/dist/jquery.validate.min.js",
                "~/wwwroot/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
                "~/wwwroot/lib/Microsoft.jQuery.Unobtrusive.Ajax/jquery.unobtrusive-ajax.min.js",
                "~/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js",
                "~/wwwroot/lib/angular/angular.min.js",
                "~/wwwroot/lib/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "~/wwwroot/scripts/shared/app.js");
        }
    }
}