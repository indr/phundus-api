namespace Phundus.Rest.Api
{
    using System;
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
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/orders/{orderId}/items")]
    public class OrdersItemsController : ApiControllerBase
    {
        private readonly IOrderQueries _orderQueries;

        public OrdersItemsController(IOrderQueries orderQueries)
        {
            AssertionConcern.AssertArgumentNotNull(orderQueries, "OrderQueries must be provided.");

            _orderQueries = orderQueries;
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(Guid orderId, OrdersItemsPostRequestContent requestContent)
        {
            var orderItemId = new OrderItemId();
            var command = new AddOrderItem(CurrentUserId, new OrderId(orderId), orderItemId,
                new ArticleId(requestContent.ArticleId), new Period(requestContent.FromUtc, requestContent.ToUtc),
                requestContent.Quantity);

            Dispatch(command);

            return Get(new OrderId(orderId), orderItemId.Id, HttpStatusCode.Created);
        }

        private HttpResponseMessage Get(OrderId orderId, Guid orderItemId, HttpStatusCode statusCode)
        {
            var order = _orderQueries.GetById(CurrentUserId, orderId);
            var item = order.Items.FirstOrDefault(p => p.LineId == orderItemId);
            if (item == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Die Position mit der Id {0} konnte nicht gefunden werden.", orderItemId.ToString("D")));

            return Request.CreateResponse(statusCode, new OrderItem
            {
                Amount = item.Quantity,
                ArticleId = item.ArticleShortId,
                FromUtc = item.FromUtc,
                IsAvailable = item.IsAvailable,
                ItemTotal = item.LineTotal,
                OrderId = item.Order.OrderShortId,
                OrderItemId = item.Id,
                Text = item.Text,
                ToUtc = item.ToUtc,
                UnitPrice = item.UnitPricePerWeek
            });
        }

        [PATCH("{orderItemId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid orderId, Guid orderItemId,
            OrdersItemsPatchRequestContent requestContent)
        {
            Dispatcher.Dispatch(new UpdateOrderItem
            {
                Amount = requestContent.Amount,
                FromUtc = requestContent.FromUtc,
                InitiatorId = CurrentUserId,
                OrderId = new OrderId(orderId),
                OrderItemId = orderItemId,
                ToUtc = requestContent.ToUtc,
                ItemTotal = requestContent.ItemTotal
            });

            return Get(new OrderId(orderId), orderItemId, HttpStatusCode.OK);
        }

        [DELETE("{orderItemId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(Guid orderId, Guid orderItemId)
        {
            Dispatcher.Dispatch(new RemoveOrderItem
            {
                InitiatorId = CurrentUserId,
                OrderId = new OrderId(orderId),
                OrderItemId = orderItemId
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }

    public class OrdersItemsPatchRequestContent
    {
        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }
    }

    public class OrdersItemsPostRequestContent
    {
        [JsonProperty("articleId")]
        public Guid ArticleId { get; set; }

        [JsonProperty("fromutc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}