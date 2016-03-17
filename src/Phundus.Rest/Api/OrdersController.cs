namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Application;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model.Pdf;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IMembershipQueries _membershipQueries;
        private readonly IOrderQueryService _orderQueryService;
        private readonly IPdfStore _pdfStore;
        private readonly IShortIdGeneratorService _shortIdGeneratorService;

        public OrdersController(IOrderQueryService orderQueryService, IPdfStore pdfStore,
            IShortIdGeneratorService shortIdGeneratorService, IMembershipQueries membershipQueries)
        {
            if (orderQueryService == null) throw new ArgumentNullException("orderQueryService");
            if (pdfStore == null) throw new ArgumentNullException("pdfStore");
            if (shortIdGeneratorService == null) throw new ArgumentNullException("shortIdGeneratorService");
            if (membershipQueries == null) throw new ArgumentNullException("membershipQueries");
            _orderQueryService = orderQueryService;
            _pdfStore = pdfStore;
            _shortIdGeneratorService = shortIdGeneratorService;
            _membershipQueries = membershipQueries;
        }

        public IMembershipQueries MembershipQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<Order> Get()
        {
            UserId queryUserId = null;
            OrganizationId queryOrganizationId = null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("userId"))
                queryUserId = new UserId(Guid.Parse(queryParams["userId"]));
            if (queryParams.ContainsKey("organizationId"))
                queryOrganizationId = new OrganizationId(Guid.Parse(queryParams["organizationId"]));

            var orders = _orderQueryService.Query(CurrentUserId, null, queryUserId, queryOrganizationId).ToList();
            return new QueryOkResponseContent<Order>(Map<IList<Order>>(orders));
        }

        [GET("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid orderId)
        {
            var order = _orderQueryService.GetById(CurrentUserId, new OrderId(orderId));
            var membership = _membershipQueries.Find(order.LessorId, order.LesseeId);

            var result = Map<OrderDetail>(order);
            result.IsMember = membership != null;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{orderId}/pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(Guid orderId)
        {
            var order = _orderQueryService.GetById(CurrentUserId, new OrderId(orderId));
            var result = _pdfStore.GetOrderPdf(new OrderId(orderId), CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(result, string.Format("Bestellung-{0}.pdf", order.OrderShortId));
        }

        [POST("")]
        public virtual OrdersPostOkResponseContent Post(OrdersPostRequestContent requestContent)
        {
            var orderId = new OrderId();
            var orderShortId = _shortIdGeneratorService.GetNext<OrderShortId>();

            var command = new CreateEmptyOrder(CurrentUserId,
                orderId, orderShortId,
                new LessorId(requestContent.OwnerId),
                new LesseeId(requestContent.LesseeId));

            Dispatch(command);

            return new OrdersPostOkResponseContent
            {
                OrderId = orderId.Id,
                OrderShortId = orderShortId.Id
            };
        }

        [PATCH("{orderId}")]
        public virtual HttpResponseMessage Patch(Guid orderId, OrdersPatchRequestContent requestContent)
        {
            if (requestContent.Status == "Rejected")
                Dispatch(new RejectOrder(CurrentUserId, new OrderId(orderId)));
            else if (requestContent.Status == "Approved")
                Dispatch(new ApproveOrder(CurrentUserId, new OrderId(orderId)));
            else if (requestContent.Status == "Closed")
                Dispatch(new CloseOrder(CurrentUserId, new OrderId(orderId)));
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Unbekannter Status \"" + requestContent.Status + "\"");

            return Get(orderId);
        }
    }

    public class OrdersPatchRequestContent
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class OrdersPostRequestContent
    {
        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("lesseeId")]
        public Guid LesseeId { get; set; }
    }

    public class OrdersPostOkResponseContent
    {
        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }

        [JsonProperty("orderShortId")]
        public int OrderShortId { get; set; }
    }
}