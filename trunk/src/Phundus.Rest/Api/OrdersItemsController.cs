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
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

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
        public virtual HttpResponseMessage Post(int orderId, OrdersItemsPostRequestContent requestContent)
        {
            var command = new AddOrderItem
            {
                Amount = requestContent.Amount,
                ArticleId = requestContent.ArticleId,
                FromUtc = requestContent.FromUtc,
                InitiatorId = CurrentUserId.Id,
                OrderId = orderId,
                ToUtc = requestContent.ToUtc
            };

            Dispatch(command);

            return Get(orderId, command.OrderItemId, HttpStatusCode.Created);
        }

        private HttpResponseMessage Get(int orderId, Guid orderItemId, HttpStatusCode statusCode)
        {
            var order = _orderQueries.GetById(new CurrentUserId(CurrentUserId.Id), new OrderId(orderId));
            var item = order.Items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Die Position mit der Id {0} konnte nicht gefunden werden.", orderItemId.ToString("D")));

            return Request.CreateResponse(statusCode, Map<OrderItem>(item));
        }

        [PATCH("{orderItemId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int orderId, Guid orderItemId,
            OrdersItemsPatchRequestContent requestContent)
        {
            Dispatcher.Dispatch(new UpdateOrderItem
            {
                Amount = requestContent.Amount,
                FromUtc = requestContent.FromUtc,
                InitiatorId = CurrentUserId.Id,
                OrderId = orderId,
                OrderItemId = orderItemId,
                ToUtc = requestContent.ToUtc,
                ItemTotal = requestContent.ItemTotal
            });

            return Get(orderId, orderItemId, HttpStatusCode.OK);
        }

        [DELETE("{orderItemId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int orderId, Guid orderItemId)
        {
            Dispatcher.Dispatch(new RemoveOrderItem
            {
                InitiatorId = CurrentUserId.Id,
                OrderId = orderId,
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
        public int ArticleId { get; set; }

        [JsonProperty("fromutc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}