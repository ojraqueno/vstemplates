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
                "~/Scripts/lib/bootstrap/dist/css/bootstrap.css",
                "~/Scripts/lib/font-awesome/css/font-awesome.min.css",
                "~/Content/site.css");

            bundles.AddScriptBundle("~/scriptbundles/core",
                "~/Scripts/lib/jquery/dist/jquery.min.js",
                "~/Scripts/lib/jquery-validation/dist/jquery.validate.min.js",
                "~/Scripts/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
                "~/Scripts/lib/Microsoft.jQuery.Unobtrusive.Ajax/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/lib/bootstrap/dist/js/bootstrap.min.js",
                "~/Scripts/lib/angular/angular.min.js",
                "~/Scripts/lib/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "~/Scripts/Shared/app.js");
        }
    }
}