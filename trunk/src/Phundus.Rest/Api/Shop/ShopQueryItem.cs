namespace Phundus.Rest.Api.Shop
{
    using Newtonsoft.Json;

    public class ShopQueryItem
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}