namespace Phundus.Rest.ContentObjects
{
    using Newtonsoft.Json;

    public class Role
    {
        [JsonProperty("bitMask")]
        public int BitMask { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}