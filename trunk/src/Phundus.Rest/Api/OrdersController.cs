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
    using Newtonsoft.Json;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model.Pdf;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IOrderQueries _orderQueries;
        private readonly IPdfStore _pdfStore;
        private readonly IShortIdGeneratorService _shortIdGeneratorService;

        public OrdersController(IOrderQueries orderQueries, IPdfStore pdfStore,
            IShortIdGeneratorService shortIdGeneratorService)
        {
            if (orderQueries == null) throw new ArgumentNullException("orderQueries");
            if (pdfStore == null) throw new ArgumentNullException("pdfStore");
            if (shortIdGeneratorService == null) throw new ArgumentNullException("shortIdGeneratorService");

            _orderQueries = orderQueries;
            _pdfStore = pdfStore;
            _shortIdGeneratorService = shortIdGeneratorService;
        }

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

            var orders = _orderQueries.Query(CurrentUserId, null, queryUserId, queryOrganizationId).ToList();
            return new QueryOkResponseContent<Order>(Map<IList<Order>>(orders));
        }

        [GET("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid orderId)
        {
            var order = _orderQueries.GetById(CurrentUserId, new OrderId(orderId));
            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetail>(order));
        }

        [GET("{orderId}/pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(Guid orderId)
        {
            var order = _orderQueries.GetById(CurrentUserId, new OrderId(orderId));
            var result = _pdfStore.GetOrderPdf(new OrderId(orderId), CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(result, string.Format("Bestellung-{0}.pdf", order.OrderShortId));
        }

        [POST("")]
        [Transaction]
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
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid orderId, OrdersPatchRequestContent requestContent)
        {
            if (requestContent.Status == "Rejected")
                Dispatch(new RejectOrder {InitiatorId = CurrentUserId, OrderId = new OrderId(orderId)});
            else if (requestContent.Status == "Approved")
                Dispatch(new ApproveOrder {InitiatorId = CurrentUserId, OrderId = new OrderId(orderId)});
            else if (requestContent.Status == "Closed")
                Dispatch(new CloseOrder {InitiatorId = CurrentUserId, OrderId = new OrderId(orderId)});
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