namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class AdminOrganization
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("establishedAtUtc")]
        public DateTime EstablishedAtUtc { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }
    }
}