namespace Phundus.Rest.Api.Shop
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Domain.Model;
    using Common.Resources;
    using Inventory.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/shop/items/{itemId}/availability-check")]
    public class ShopItemsAvailabilityCheckController : ApiControllerBase
    {
        private readonly IAvailabilityQueryService _availabilityQueryService;

        public ShopItemsAvailabilityCheckController(IAvailabilityQueryService availabilityQueryService)
        {
            _availabilityQueryService = availabilityQueryService;
        }

        [POST("")]
        [AllowAnonymous]
        public virtual ShopItemsAvailabilityCheckOkResponseContent Post(Guid itemId,
            ShopItemsAvailabilityCheckRequestContent requestContent)
        {
            var isAvailable = _availabilityQueryService.IsArticleAvailable(new ArticleId(requestContent.ItemId),
                requestContent.FromUtc,
                requestContent.ToUtc, requestContent.Quantity);

            return new ShopItemsAvailabilityCheckOkResponseContent {IsAvailable = isAvailable};
        }

        public class ShopItemsAvailabilityCheckRequestContent
        {
            [JsonProperty("itemId")]
            public Guid ItemId { get; set; }

            [JsonProperty("quantity")]
            public int Quantity { get; set; }

            [JsonProperty("fromUtc")]
            public DateTime FromUtc { get; set; }

            [JsonProperty("toUtc")]
            public DateTime ToUtc { get; set; }
        }

        public class ShopItemsAvailabilityCheckOkResponseContent
        {
            [JsonProperty("isAvailable")]
            public bool IsAvailable { get; set; }
        }
    }
}