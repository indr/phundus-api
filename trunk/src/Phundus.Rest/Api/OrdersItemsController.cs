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
    using Phundus.Shop.Application;
    using Phundus.Shop.Projections;

    [RoutePrefix("api/orders/{orderId}/items")]
    public class OrdersItemsController : ApiControllerBase
    {
        private readonly IOrderQueries _orderQueries;

        public OrdersItemsController(IOrderQueries orderQueries)
        {
            if (orderQueries == null) throw new ArgumentNullException("orderQueries");
            _orderQueries = orderQueries;
        }

        [POST("")]        
        public virtual HttpResponseMessage Post(Guid orderId, OrdersItemsPostRequestContent rq)
        {
            var orderItemId = new OrderLineId();
            var command = new AddOrderItem(CurrentUserId, new OrderId(orderId), orderItemId,
                new ArticleId(rq.ArticleId), new Period(rq.FromUtc, rq.ToUtc),
                rq.Quantity, rq.LineTotal);

            Dispatch(command);

            return Accepted();
        }

        private HttpResponseMessage Get(OrderId orderId, Guid orderItemId, HttpStatusCode statusCode)
        {
            var order = _orderQueries.GetById(CurrentUserId, orderId);
            var item = order.Lines.FirstOrDefault(p => p.LineId == orderItemId);
            if (item == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Order line item {0} not found.", orderItemId.ToString("D")));

            return Request.CreateResponse(statusCode, Map<OrderItem>(item));
        }

        [PATCH("{orderItemId}")]        
        public virtual HttpResponseMessage Patch(Guid orderId, Guid orderItemId,
            OrdersItemsPatchRequestContent rq)
        {
            Dispatch(new UpdateOrderItem(CurrentUserId, new OrderId(orderId), orderItemId, new Period(rq.FromUtc, rq.ToUtc), rq.Quantity, rq.LineTotal));

            return Accepted();
        }

        [DELETE("{orderItemId}")]        
        public virtual HttpResponseMessage Delete(Guid orderId, Guid orderItemId)
        {
            Dispatch(new RemoveOrderItem(CurrentUserId, new OrderId(orderId), orderItemId));

            return Accepted();
        }
    }

    public class OrdersItemsPatchRequestContent
    {
        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("lineTotal")]
        public decimal LineTotal { get; set; }
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

        [JsonProperty("lineTotal")]
        public decimal LineTotal { get; set; }
    }
}