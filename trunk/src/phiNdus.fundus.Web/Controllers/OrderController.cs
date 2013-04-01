using System.Web.Mvc;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize]
    public class OrderController : ControllerBase
    {
        //
        // GET: /Order/
        public ActionResult Index()
        {
            return View("My", new MyOrdersViewModel());
        }

        //
        // GET: /Order/Pending
        [Authorize(Roles = "Chief")]
        public ActionResult Pending()
        {
            return View("Pending", new OrdersViewModel(OrderStatus.Pending));
        }

        //
        // GET: /Order/Approved
        [Authorize(Roles = "Chief")]
        public ActionResult Approved()
        {
            return View("Approved", new OrdersViewModel(OrderStatus.Approved));
        }

        //
        // GET: /Order/Closed
        [Authorize(Roles = "Chief")]
        public ActionResult Closed()
        {
            return View("Closed", new OrdersViewModel(OrderStatus.Closed));
        }

        //
        // GET: /Order/Rejected
        [Authorize(Roles = "Chief")]
        public ActionResult Rejected()
        {
            return View("Rejected", new OrdersViewModel(OrderStatus.Rejected));
        }

        public ActionResult Details(int id)
        {
            var model = new OrderViewModel(id);

            return View("Details", model);
        }

        [HttpPost]
        [Authorize(Roles = "Chief")]
        public ActionResult Reject(int id)
        {
            var service = IoC.Resolve<IOrderService>();
            var orderDto = service.Reject(Session.SessionID, id);
            return Json(orderDto);
        }

        [HttpPost]
        [Authorize(Roles = "Chief")]
        public ActionResult Confirm(int id)
        {
            var service = IoC.Resolve<IOrderService>();
            var orderDto = service.Confirm(Session.SessionID, id);
            return Json(orderDto);
        }

        [HttpGet]
        [Authorize]
        public FileStreamResult Print(int id)
        {
            var service = IoC.Resolve<IOrderService>();
            var stream = service.GetPdf(Session.SessionID, id);

            return new FileStreamResult(stream, "application/pdf")
                             {
                                 FileDownloadName = string.Format("Order-{0}.pdf", id)
                             };
        }
    }
}