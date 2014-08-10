namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Model;
    using Core.Shop.Queries;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Web.ViewModels;

    [Authorize]
    public class OrderController : ControllerBase
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        //
        // GET: /Order/
        [Transaction]
        public virtual ActionResult Index()
        {
            return View("My", new MyOrdersViewModel());
        }

        //
        // GET: /Order/Pending
        [Transaction]
        public virtual ActionResult Pending()
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new OrdersViewModel(
                OrderQueries.FindByOrganizationId(CurrentOrganizationId.Value, CurrentUserId,
                    OrderStatus.Pending));
            return View("Pending", model);
        }

        //
        // GET: /Order/Approved
        [Transaction]
        public virtual ActionResult Approved()
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new OrdersViewModel(
                OrderQueries.FindByOrganizationId(CurrentOrganizationId.Value, CurrentUserId,
                    OrderStatus.Approved));
            return View("Approved", model);
        }

        //
        // GET: /Order/Closed
        [Transaction]
        public virtual ActionResult Closed()
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new OrdersViewModel(
                OrderQueries.FindByOrganizationId(CurrentOrganizationId.Value, CurrentUserId,
                    OrderStatus.Closed));
            return View("Closed", model);
        }

        //
        // GET: /Order/Rejected
        [Transaction]
        public virtual ActionResult Rejected()
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var model = new OrdersViewModel(
                OrderQueries.FindByOrganizationId(CurrentOrganizationId.Value, CurrentUserId,
                    OrderStatus.Rejected)); ;
            return View("Rejected", model);
        }

        [Transaction]
        public virtual ActionResult Details(int id)
        {
            // TODO: Authorization

            var model = new OrderViewModel(id);

            return View("Details", model);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Reject(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var service = ServiceLocator.Current.GetInstance<IOrderService>();
            var orderDto = service.Reject(id);
            return Json(orderDto);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Confirm(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            var service = ServiceLocator.Current.GetInstance<IOrderService>();
            var orderDto = service.Confirm(id);
            return Json(orderDto);
        }

        [HttpGet]
        [Authorize]
        public FileStreamResult Print(int id)
        {
            // TODO: Authorization

            var service = ServiceLocator.Current.GetInstance<IOrderService>();
            var stream = service.GetPdf(id);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = string.Format("Order-{0}.pdf", id)
            };
        }
    }
}