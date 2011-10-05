using System.Web.Mvc;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService OrderService
        {
            get { return IoC.Resolve<IOrderService>(); }
        }

        //
        // GET: /Order/
        public ActionResult Index()
        {
            return RedirectToAction("Pending");
        }

        //
        // GET: /Order/Pending
        public ActionResult Pending()
        {
            return View("Pending", "_Orders", new OrdersTableViewModel
                                       {
                                           Orders = OrderService.GetPendingOrders(Session.SessionID)
                                       });
        }

        //
        // GET: /Order/Approved
        public ActionResult Approved()
        {
            return View("Approved", "_Orders", new OrdersTableViewModel
                                        {
                                            Orders = OrderService.GetApprovedOrders(Session.SessionID)
                                        });
        }

        //
        // GET: /Order/Rejected
        public ActionResult Rejected()
        {
            return View("Rejected", "_Orders", new OrdersTableViewModel
                                        {
                                            Orders = OrderService.GetRejectedOrders(Session.SessionID)
                                        });
        }
    }
}