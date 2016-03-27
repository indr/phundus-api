namespace Phundus.Shop.Resources
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Domain.Model;
    using Common.Resources;
    using Inventory.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/shop/products/{productId}/availability-check")]
    public class ShopProductsAvailabilityCheckController : ApiControllerBase
    {
        private readonly IAvailabilityQueryService _availabilityQueryService;

        public ShopProductsAvailabilityCheckController(IAvailabilityQueryService availabilityQueryService)
        {
            _availabilityQueryService = availabilityQueryService;
        }

        [POST("")]
        [AllowAnonymous]
        public virtual PostOkResponseContent Post(ArticleId productId, PostRequestContent rq)
        {
            var quantityPeriods = rq.Items.Select(s => new QuantityPeriod(s.FromUtc, s.ToUtc, s.Quantity)).ToArray();

            var isAvailable = _availabilityQueryService.IsAvailable(productId, quantityPeriods);

            return new PostOkResponseContent { IsAvailable = isAvailable };
        }

        public class PostRequestContent
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }

            public class Item
            {
                [JsonProperty("fromUtc")]
                public DateTime FromUtc { get; set; }

                [JsonProperty("toUtc")]
                public DateTime ToUtc { get; set; }

                [JsonProperty("quantity")]
                public int Quantity { get; set; }
            } 
        }

        public class PostOkResponseContent
        {
            [JsonProperty("isAvailable")]
            public bool IsAvailable { get; set; }
        }
    }
}