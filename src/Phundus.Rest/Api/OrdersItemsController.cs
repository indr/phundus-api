namespace Phundus.Rest.Api
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Domain.Model;
    using Common.Resources;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/orders/{orderId}/items")]
    public class OrdersItemsController : ApiControllerBase
    {
        [POST("")]
        public virtual HttpResponseMessage Post(Guid orderId, OrdersItemsPostRequestContent rq)
        {
            var orderLineId = new OrderLineId();
            var command = new AddOrderItem(CurrentUserId, new OrderId(orderId), orderLineId,
                new ArticleId(rq.ArticleId), new Period(rq.FromUtc, rq.ToUtc), rq.Quantity, rq.LineTotal);

            Dispatch(command);

            return NoContent();
        }

        [PATCH("{orderItemId}")]
        public virtual HttpResponseMessage Patch(Guid orderId, Guid orderItemId,
            OrdersItemsPatchRequestContent rq)
        {
            Dispatch(new UpdateOrderItem(CurrentUserId, new OrderId(orderId), orderItemId,
                new Period(rq.FromUtc, rq.ToUtc), rq.Quantity, rq.LineTotal));

            return NoContent();
        }

        [DELETE("{orderItemId}")]
        public virtual HttpResponseMessage Delete(Guid orderId, Guid orderItemId)
        {
            Dispatch(new RemoveOrderItem(CurrentUserId, new OrderId(orderId), orderItemId));

            return NoContent();
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