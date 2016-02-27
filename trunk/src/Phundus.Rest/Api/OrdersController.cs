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
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using Newtonsoft.Json;
    using Phundus.Shop.Model.Pdf;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IOrdersQueries _ordersQueries;
        private readonly IPdfStore _pdfStore;

        public OrdersController(IOrdersQueries ordersQueries, IPdfStore pdfStore)
        {
            AssertionConcern.AssertArgumentNotNull(ordersQueries, "OrderQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(pdfStore, "PdfStore must be provided.");

            _ordersQueries = ordersQueries;
            _pdfStore = pdfStore;
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

            var orders = _ordersQueries.Query(CurrentUserId, null, queryUserId, queryOrganizationId).ToList();
            return new QueryOkResponseContent<Order>(Map<IList<Order>>(orders));
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int orderId)
        {
            var order = _ordersQueries.GetById(CurrentUserId, new ShortOrderId(orderId));
            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetail>(order));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int orderId)
        {
            _ordersQueries.GetById(CurrentUserId, new ShortOrderId(orderId));
            var result = _pdfStore.GetOrderPdf(orderId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(result, string.Format("Bestellung-{0}.pdf", orderId));
        }

        [POST("")]
        [Transaction]
        public virtual OrdersPostOkResponseContent Post(OrdersPostRequestContent requestContent)
        {
            var command = new CreateEmptyOrder(CurrentUserId,
                new LessorId(requestContent.OwnerId),
                new LesseeId(requestContent.LesseeId));

            Dispatch(command);

            return new OrdersPostOkResponseContent
            {
                OrderId = command.ResultingOrderId
            };
        }

        [PATCH("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int orderId, OrdersPatchRequestContent requestContent)
        {
            if (requestContent.Status == "Rejected")
                Dispatch(new RejectOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else if (requestContent.Status == "Approved")
                Dispatch(new ApproveOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else if (requestContent.Status == "Closed")
                Dispatch(new CloseOrder {InitiatorId = CurrentUserId, OrderId = orderId});
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
        public int OrderId { get; set; }
    }
}