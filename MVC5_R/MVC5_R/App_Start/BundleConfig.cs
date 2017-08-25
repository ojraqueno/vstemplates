using MVC5_R.Infrastructure.Bundling;
using System.Web.Optimization;

namespace MVC5_R
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
                "~/bower_components/bootstrap/dist/css/bootstrap.css",
                "~/bower_components/font-awesome/css/font-awesome.min.css",
                "~/Content/site.css");

            bundles.AddScriptBundle("~/scriptbundles/core",
                "~/bower_components/jquery/dist/jquery.min.js",
                "~/bower_components/jquery-validation/dist/jquery.validate.min.js",
                "~/bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
                "~/bower_components/Microsoft.jQuery.Unobtrusive.Ajax/jquery.unobtrusive-ajax.min.js",
                "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                "~/bower_components/angular/angular.min.js",
                "~/bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "~/Scripts/Shared/app.js");
        }
    }
}