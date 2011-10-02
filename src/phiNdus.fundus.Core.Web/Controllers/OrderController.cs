using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Order/List
        public ActionResult List()
        {
            return View();
        }
    }
}