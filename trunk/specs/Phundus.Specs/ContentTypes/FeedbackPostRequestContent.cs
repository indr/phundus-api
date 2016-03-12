namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class FeedbackPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    public class LessorDetails
    {
        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("postalAddress")]
        public string PostalAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("publicRental")]
        public bool PublicRental { get; set; }
    }
}