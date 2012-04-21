using System.Web.Mvc;
using phiNdus.fundus.Web.ViewModels;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        public ActionResult Index()
        {
            return View("My", new MyOrdersViewModel());
        }

        //
        // GET: /Order/Pending
        [Authorize(Roles = "Admin")]
        public ActionResult Pending()
        {
            return View("Pending", new OrdersViewModel());
        }

        //
        // GET: /Order/Approved
        [Authorize(Roles = "Admin")]
        public ActionResult Approved()
        {
            return View("Approved", new OrdersViewModel());
        }

        //
        // GET: /Order/Closed
        [Authorize(Roles = "Admin")]
        public ActionResult Closed()
        {
            return View("Closed", new OrdersViewModel());
        }

        //
        // GET: /Order/Rejected
        [Authorize(Roles = "Admin")]
        public ActionResult Rejected()
        {
            return View("Rejected", new OrdersViewModel());
        }
    }
}