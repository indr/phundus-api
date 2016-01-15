namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class ArticlesPostOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }

    public class ArticlesPostRequestContent
    {
        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class Article
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("grossStock")]
        public int GrossStock { get; set; }

        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }
    }
}