using System.Web.Mvc;

namespace MVC5_R.WebAPI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}