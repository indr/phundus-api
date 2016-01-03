namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class OrderItem
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("orderItemId")]
        public Guid OrderItemId { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }
    }
}