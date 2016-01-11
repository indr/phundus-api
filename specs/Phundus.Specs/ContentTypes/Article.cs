namespace Phundus.Specs.ContentTypes
{
    using System.Linq;
    using Newtonsoft.Json;

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
    }
}