﻿namespace Phundus.Specs.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Order
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("items")]
        public List<OrderItems> Items { get; set; }
    }

    public class OrderItems
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("unitPricePerWeek")]
        public decimal UnitPricePerWeek { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }
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
        public Guid ArticleId { get; set; }

        [JsonProperty("fromutc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}