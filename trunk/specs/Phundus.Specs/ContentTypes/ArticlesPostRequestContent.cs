namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class ArticlesPostRequestContent
    {
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}