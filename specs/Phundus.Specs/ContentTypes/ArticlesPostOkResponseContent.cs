namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class ArticlesPostOkResponseContent
    {
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
    }
}