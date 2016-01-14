namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class Order
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
    }

    internal class OrdersPostOkResponseContent
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
    }

    internal class OrdersItemsPostRequestContent
    {
        public int OrderId { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("fromutc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("amount")]
        public int Quantity { get; set; }
    }
}