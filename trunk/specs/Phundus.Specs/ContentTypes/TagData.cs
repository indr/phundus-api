namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class TagData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}