namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class ShopItemsAvailabilityCheckOkResponseContent
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }
    }

    public class ShopOrdersPostRequestContent
    {
        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }
    }

    public class ShopOrdersPostOkResponseContent
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
    }
}