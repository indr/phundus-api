using System.Web.Mvc;

namespace phiNdus.fundus.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}