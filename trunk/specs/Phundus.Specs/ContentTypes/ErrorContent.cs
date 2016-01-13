namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class ErrorContent
    {
        [JsonProperty("message")]
        public string Msg { get; set; }
    }
}