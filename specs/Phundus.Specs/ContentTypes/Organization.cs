namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class Organization
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}