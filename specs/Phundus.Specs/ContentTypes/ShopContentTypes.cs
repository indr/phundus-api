namespace Phundus.Specs.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using NUnit.Framework;

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
        public Guid OrderId { get; set; }

        [JsonProperty("orderShortId")]
        public int OrderShortId { get; set; }
    }

    public class ShopItem
    {
        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("publicPrice")]
        public decimal PublicPrice { get; set; }

        [JsonProperty("memberPrice")]
        public decimal? MemberPrice { get; set; }

        [JsonProperty("documents")]
        public List<Document> Documents { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        public class Document
        {
            [JsonProperty("fileName")]
            public string FileName { get; set; }
        }

        public class Image
        {
            [JsonProperty("fileName")]
            public string FileName { get; set; }
        }
    }
}