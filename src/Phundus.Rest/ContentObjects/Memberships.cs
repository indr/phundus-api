namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class Memberships
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("organizationName")]
        public string OrganizationName { get; set; }

        [JsonProperty("organizationUrl")]
        public string OrganizationUrl { get; set; }

        [JsonProperty("isManager")]
        public bool? IsManager { get; set; }
    }
}