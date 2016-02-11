namespace Phundus.Rest.Api.Shop
{
    using System;
    using Newtonsoft.Json;

    public class ShopQueryItem
    {
        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("itemShortId")]
        public int ItemShortId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("publicPrice")]
        public decimal PublicPrice { get; set; }

        [JsonProperty("memberPrice")]
        public decimal? MemberPrice { get; set; }

        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }

        [JsonProperty("lessorName")]
        public string LessorName { get; set; }

        [JsonProperty("lessorType")]
        public int LessorType { get; set; }

        [JsonProperty("previewImageUrl")]
        public string PreviewImageUrl { get; set; }
    }
}