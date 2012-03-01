using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class ContractController : Controller
    {
        //
        // GET: /Contract/
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Contract/List
        public ActionResult List()
        {
            return View();
        }

        public ActionResult My()
        {
            return RedirectToAction("List");
        }

        public ActionResult Signed()
        {
            return RedirectToAction("List");
        }

        public ActionResult Closed()
        {
            return RedirectToAction("List");
        }
    }
}