namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class Member
    {
        public Guid Guid { get; set; }
        public int MemberVersion { get; set; }
        public int MembershipVersion { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class OrganizationsRelationshipsQueryOkResponseContent
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