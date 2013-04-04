namespace phiNdus.fundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using phiNdus.fundus.Business.SecuredServices;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Web.ViewModels;
    using piNuts.phundus.Infrastructure.Obsolete;

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
            return View("Pending", new OrdersViewModel(OrderStatus.Pending));
        }

        //
        // GET: /Order/Approved
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Approved()
        {
            return View("Approved", new OrdersViewModel(OrderStatus.Approved));
        }

        //
        // GET: /Order/Closed
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Closed()
        {
            return View("Closed", new OrdersViewModel(OrderStatus.Closed));
        }

        //
        // GET: /Order/Rejected
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Rejected()
        {
            return View("Rejected", new OrdersViewModel(OrderStatus.Rejected));
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
            var service = GlobalContainer.Resolve<IOrderService>();
            var orderDto = service.Reject(Session.SessionID, id);
            return Json(orderDto);
        }

        [HttpPost]
        [Authorize(Roles = "Chief")]
        [Transaction]
        public virtual ActionResult Confirm(int id)
        {
            var service = GlobalContainer.Resolve<IOrderService>();
            var orderDto = service.Confirm(Session.SessionID, id);
            return Json(orderDto);
        }

        [HttpGet]
        [Authorize]
        public FileStreamResult Print(int id)
        {
            var service = GlobalContainer.Resolve<IOrderService>();
            var stream = service.GetPdf(Session.SessionID, id);

            return new FileStreamResult(stream, "application/pdf")
                       {
                           FileDownloadName = string.Format("Order-{0}.pdf", id)
                       };
        }
    }
}