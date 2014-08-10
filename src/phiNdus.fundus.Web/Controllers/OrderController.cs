namespace Phundus.Web.Controllers
{
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Model;
    using Core.Shop.Queries;
    using phiNdus.fundus.Web.ViewModels;

    [Authorize]
    public class OrderController : ControllerBase
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderService OrderService { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        //
        // GET: /Order/
        [Transaction]
        public virtual ActionResult Index()
        {
            var orders = OrderQueries.FindByUserId(CurrentUserId);

            return View("My", new MyOrdersViewModel(orders));
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
                    OrderStatus.Rejected));
            return View("Rejected", model);
        }

        [Transaction]
        public virtual ActionResult Details(int id)
        {
            // TODO: Authorization

            var dto = OrderQueries.FindById(id, CurrentUserId);
            var model = new OrderViewModel(dto);

            return View("Details", model);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Reject(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            OrderService.Reject(id);

            var orderDto = OrderQueries.FindById(id, CurrentUserId);
            return Json(orderDto);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult Confirm(int id)
        {
            MemberInRole.ActiveChief(CurrentOrganizationId.Value, CurrentUserId);

            OrderService.Confirm(id);

            var orderDto = OrderQueries.FindById(id, CurrentUserId);
            return Json(orderDto);
        }

        [HttpGet]
        [Authorize]
        public FileStreamResult Print(int id)
        {
            // TODO: Authorization

            var stream = OrderService.GetPdf(id);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = string.Format("Order-{0}.pdf", id)
            };
        }
    }
}