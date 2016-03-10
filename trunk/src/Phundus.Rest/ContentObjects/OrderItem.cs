namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class OrderItem
    {
        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }

        [JsonProperty("orderShortId")]
        public int OrderShortId { get; set; }

        [JsonProperty("orderItemId")]
        public Guid OrderItemId { get; set; }

        [JsonProperty("articleId")]
        public Guid ArticleId { get; set; }

        [JsonProperty("articleShortId")]
        public int ArticleShortId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("lineTotal")]
        public decimal LineTotal { get; set; }
    }
}