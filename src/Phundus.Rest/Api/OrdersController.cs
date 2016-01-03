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
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IOrderQueries _orderQueries;
        private readonly IPdfStore _pdfStore;

        public OrdersController(IOrderQueries orderQueries, IPdfStore pdfStore)
        {
            AssertionConcern.AssertArgumentNotNull(orderQueries, "OrderQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(pdfStore, "PdfStore must be provided.");

            _orderQueries = orderQueries;
            _pdfStore = pdfStore;
        }

        [GET("")]
        [Transaction]
        public virtual OrdersQueryOkResponseContent Get()
        {
            var userId = (int?) null;
            var organizationId = (Guid?) null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("userId"))
                userId = Convert.ToInt32(queryParams["userId"]);
            if (queryParams.ContainsKey("organizationId"))
                organizationId = Guid.Parse(queryParams["organizationId"]);

            var orders = _orderQueries.Query(new CurrentUserId(CurrentUserId.Id), null, userId, organizationId).ToList();
            var result = new OrdersQueryOkResponseContent
            {
                Orders = Map<IList<Order>>(orders)
            };
            return result;
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int orderId)
        {
            var order = _orderQueries.GetById(new CurrentUserId(CurrentUserId.Id), new OrderId(orderId));
            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetail>(order));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int orderId)
        {
            _orderQueries.GetById(new CurrentUserId(CurrentUserId.Id), new OrderId(orderId));
            var result = _pdfStore.GetOrderPdf(orderId, CurrentUserId.Id);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(result, string.Format("Bestellung-{0}.pdf", orderId));
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(OrdersPostRequestContent requestContent)
        {
            var command = new CreateEmptyOrder
            {
                InitiatorId = new CurrentUserId(CurrentUserId.Id),
                LessorId = new LessorId(requestContent.OwnerId),
                LesseeId = new LesseeId(requestContent.LesseeId)
            };

            Dispatcher.Dispatch(command);

            return Get(command.ResultingOrderId);
        }

        [PATCH("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int orderId, OrdersPatchRequestContent requestContent)
        {
            if (requestContent.Status == "Rejected")
                Dispatch(new RejectOrder {InitiatorId = CurrentUserId.Id, OrderId = orderId});
            else if (requestContent.Status == "Approved")
                Dispatch(new ApproveOrder {InitiatorId = CurrentUserId.Id, OrderId = orderId});
            else if (requestContent.Status == "Closed")
                Dispatch(new CloseOrder {InitiatorId = CurrentUserId.Id, OrderId = orderId});
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
        public int LesseeId { get; set; }
    }

    public class OrdersQueryOkResponseContent
    {
        [JsonProperty("orders")]
        public IList<Order> Orders { get; set; }
    }
}