namespace Phundus.Rest.Api
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Domain.Model;
    using Common.Resources;
    using Inventory.Application;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/orders/{orderId}/items")]
    public class OrdersItemsController : ApiControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public OrdersItemsController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [POST("")]
        public virtual HttpResponseMessage Post(Guid orderId, PostRequestContent rq)
        {
            var orderLineId = new OrderLineId();
            var articleId = new ArticleId(rq.ArticleId);

            Dispatch(new AddOrderItem(CurrentUserId, new OrderId(orderId), orderLineId, articleId,
                new Period(rq.FromUtc, rq.ToUtc), rq.Quantity, rq.LineTotal));

            var isAvailable = _availabilityService.IsArticleAvailable(articleId, rq.FromUtc, rq.ToUtc, rq.Quantity,
                orderLineId);

            return Created(new PostCreatedResponseContent {OrderLineId = orderLineId.Id, IsAvailable = isAvailable});
        }

        [PATCH("{orderItemId}")]
        public virtual HttpResponseMessage Patch(Guid orderId, Guid orderItemId,
            PatchRequestContent rq)
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

        public class PatchRequestContent
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

        public class PostCreatedResponseContent
        {
            [JsonProperty("orderLineId")]
            public Guid OrderLineId { get; set; }

            [JsonProperty("isAvailable")]
            public bool IsAvailable { get; set; }
        }

        public class PostRequestContent
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
}