using System.Web.Mvc;

namespace MVC5_R.Controllers
{
    public class MVC5_RController : Controller
    {
        protected string BaseUrl
        {
            get
            {
                return $"{Request.Url.Scheme}://{Request.Url.Authority}{Request.ApplicationPath.TrimEnd('/')}";
            }
        }

        protected new RedirectToRouteResult RedirectToAction(string actionName, string controllerName)
        {
            var formattedControllerName = controllerName.Contains("Controller") ? controllerName.Substring(0, controllerName.Length - 10) : controllerName;

            return base.RedirectToAction(actionName, formattedControllerName);
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }
    }
}