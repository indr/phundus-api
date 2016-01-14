namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    internal class Order
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
    }

    internal class OrdersPostOkResponseContent
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
    }
}