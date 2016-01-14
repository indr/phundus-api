namespace Phundus.Rest.Api.Shop
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Inventory.Services;
    using Newtonsoft.Json;

    [RoutePrefix("api/shop/items/{itemId}/availability-check")]
    public class ShopItemsAvailabilityCheckController : ApiControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public ShopItemsAvailabilityCheckController(IAvailabilityService availabilityService)
        {
            if (availabilityService == null) throw new ArgumentNullException("availabilityService");
            _availabilityService = availabilityService;
        }

        [POST("")]
        [Transaction]
        [AllowAnonymous]
        public virtual ShopItemsAvailabilityCheckOkResponseContent Post(int itemId,
            ShopItemsAvailabilityCheckRequestContent requestContent)
        {
            var isAvailable = _availabilityService.IsArticleAvailable(itemId, requestContent.FromUtc,
                requestContent.ToUtc,
                requestContent.Quantity);            
            return new ShopItemsAvailabilityCheckOkResponseContent {IsAvailable = isAvailable};
        }
    }

    public class ShopItemsAvailabilityCheckRequestContent
    {
        [JsonProperty("itemId")]
        public int ItemId { get; set; }

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