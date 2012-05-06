using System.Web.Mvc;
using phiNdus.fundus.Domain.Entities;
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
            return View("Pending", new OrdersViewModel(OrderStatus.Pending));
        }

        //
        // GET: /Order/Approved
        [Authorize(Roles = "Admin")]
        public ActionResult Approved()
        {
            return View("Approved", new OrdersViewModel(OrderStatus.Approved));
        }

        //
        // GET: /Order/Closed
        [Authorize(Roles = "Admin")]
        public ActionResult Closed()
        {
            return View("Closed", new OrdersViewModel(OrderStatus.Closed));
        }

        //
        // GET: /Order/Rejected
        [Authorize(Roles = "Admin")]
        public ActionResult Rejected()
        {
            return View("Rejected", new OrdersViewModel(OrderStatus.Rejected));
        }

        public ActionResult Details(int id)
        {
            var model = new OrderViewModel(id);

            return View("Details", model);
        }
    }
}