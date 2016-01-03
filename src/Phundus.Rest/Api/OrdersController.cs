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
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IOrderQueries _orderQueries;
        private readonly IPdfStore _pdfStore;
        private readonly IUserQueries _userQueries;

        public OrdersController(IUserQueries userQueries, IOrderQueries orderQueries, IPdfStore pdfStore)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderQueries, "OrderQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(pdfStore, "PdfStore must be provided.");

            _userQueries = userQueries;
            _orderQueries = orderQueries;
            _pdfStore = pdfStore;
        }


        [GET("")]
        [Transaction]
        public virtual OrdersQueryOkResponseContent Get()
        {
            var userId = (int?) null;
            var organizationId = (Guid?) null;

            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(ks => ks.Key, es => es.Value);
            if (queryParams.ContainsKey("userId"))
                userId = Convert.ToInt32(queryParams["userId"]);
            if (queryParams.ContainsKey("organizationId"))
                organizationId = Guid.Parse(queryParams["organizationId"]);

            var orders = _orderQueries.Query(new CurrentUserId(CurrentUserId), null, userId, organizationId).ToList();
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
            var order = _orderQueries.GetById(new CurrentUserId(CurrentUserId), new OrderId(orderId));
            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetail>(order));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int orderId)
        {
            _orderQueries.GetById(new CurrentUserId(CurrentUserId), new OrderId(orderId));
            var result = _pdfStore.GetOrderPdf(orderId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(result, string.Format("Bestellung-{0}.pdf", orderId));
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(OrdersPostRequestContent requestContent)
        {
            int userId;
            if (!Int32.TryParse(requestContent.Username, out userId))
            {
                var user = _userQueries.ByUserName(requestContent.Username);
                if (user == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        string.Format("Der Benutzer mit der E-Mail-Adresse \"{0}\" konnte nicht gefunden werden.",
                            requestContent.Username));

                userId = user.Id;
            }

            var command = new CreateEmptyOrder
            {
                InitiatorId = new CurrentUserId(CurrentUserId),
                LessorId = new LessorId(requestContent.OwnerId),
                LesseeId = new LesseeId(userId)
            };

            Dispatcher.Dispatch(command);

            return Get(command.ResultingOrderId);
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

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class OrdersQueryOkResponseContent
    {
        [JsonProperty("orders")]
        public IList<Order> Orders { get; set; }
    }
}