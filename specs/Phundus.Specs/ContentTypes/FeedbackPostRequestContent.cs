namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class FeedbackPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}