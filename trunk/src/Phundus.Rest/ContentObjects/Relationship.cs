namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class Relationship
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}