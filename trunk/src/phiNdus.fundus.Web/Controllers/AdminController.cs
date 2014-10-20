namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;

    public class AdminController : ControllerBase
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}