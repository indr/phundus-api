namespace Phundus.Specs.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class UsersCartGetOkResponseContent
    {
        [JsonProperty("items")]
        public List<CartItems> Items { get; set; }
    }

    public class CartItems
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
    }

    public class UserCartItemsPostOkResponseContent
    {
        [JsonProperty("cartItemId")]
        public Guid CartItemId { get; set; }
    }
}