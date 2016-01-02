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
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;
    using Newtonsoft.Json;
    using Organizations;
    using OrderDetailDoc = Docs.OrderDetailDoc;
    using OrderDoc = Docs.OrderDoc;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        public IUserQueries UserQueries { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        public IPdfStore PdfStore { get; set; }

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

            var orders = OrderQueries.Query(new CurrentUserId(CurrentUserId), null, userId, organizationId).ToList();
            var result = new OrdersQueryOkResponseContent
            {
                Orders = Map<IList<OrderDoc>>(orders)
            };
            return result;
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int orderId)
        {
            var order = OrderQueries.GetById(new CurrentUserId(CurrentUserId), new OrderId(orderId));
            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetailDoc>(order));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int orderId)
        {
            OrderQueries.GetById(new CurrentUserId(CurrentUserId), new OrderId(orderId));
            var result = PdfStore.GetOrderPdf(orderId, CurrentUserId);
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
                var user = UserQueries.ByUserName(requestContent.Username);
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
        public virtual HttpResponseMessage Patch(int orderId, OrderPatchDoc doc)
        {
            if (doc.Status == "Rejected")
                Dispatch(new RejectOrder { InitiatorId = CurrentUserId, OrderId = orderId });
            else if (doc.Status == "Approved")
                Dispatch(new ApproveOrder { InitiatorId = CurrentUserId, OrderId = orderId });
            else if (doc.Status == "Closed")
                Dispatch(new CloseOrder { InitiatorId = CurrentUserId, OrderId = orderId });
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Unbekannter Status \"" + doc.Status + "\"");

            return Get(orderId);
        }

    }

    public class OrdersQueryOkResponseContent
    {
        [JsonProperty("orders")]
        public IList<OrderDoc> Orders { get; set; }
    }

    public class Order
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonProperty("lessorName")]
        public string LessorName { get; set; }

        [JsonProperty("lesseeName")]
        public string LesseeName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}