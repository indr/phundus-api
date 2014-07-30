namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Business.Services;
    using Castle.Transactions;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Web.ViewModels;
    using Phundus.Core.ReservationCtx;
    using Phundus.Core.ReservationCtx.Model;
    using Phundus.Core.Shop.Orders;
    using Phundus.Core.Shop.Orders.Model;

    [Authorize]
    public class OrderController : ControllerBase
    {
        //
        // GET: /Order/
        [Transaction]
        public virtual ActionResult Index()
        {
            return View("My", new MyOrdersViewModel());
        }

        //
        // GET: /Order/Pending
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Pending()
        {
            return View("Pending", new OrdersViewModel(OrderStatus.Pending, CurrentOrganizationId.Value));
        }

        //
        // GET: /Order/Approved
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Approved()
        {
            return View("Approved", new OrdersViewModel(OrderStatus.Approved, CurrentOrganizationId.Value));
        }

        //
        // GET: /Order/Closed
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Closed()
        {
            return View("Closed", new OrdersViewModel(OrderStatus.Closed, CurrentOrganizationId.Value));
        }

        //
        // GET: /Order/Rejected
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Rejected()
        {
            return View("Rejected", new OrdersViewModel(OrderStatus.Rejected, CurrentOrganizationId.Value));
        }

        [Transaction]
        public virtual ActionResult Details(int id)
        {
            var model = new OrderViewModel(id);

            return View("Details", model);
        }

        [HttpPost]
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Reject(int id)
        {
            var service = ServiceLocator.Current.GetInstance<IOrderService>();
            var orderDto = service.Reject(id);
            return Json(orderDto);
        }

        [HttpPost]
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Confirm(int id)
        {
            var service = ServiceLocator.Current.GetInstance<IOrderService>();
            var orderDto = service.Confirm(id);
            return Json(orderDto);
        }

        [HttpGet]
        [Authorize]
        public FileStreamResult Print(int id)
        {
            var service = ServiceLocator.Current.GetInstance<IOrderService>();
            var stream = service.GetPdf(id);

            return new FileStreamResult(stream, "application/pdf")
                       {
                           FileDownloadName = string.Format("Order-{0}.pdf", id)
                       };
        }
    }
}