namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class ShopItemsAvailabilityCheckOkResponseContent
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }
    }
}