namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class Member
    {
        [JsonProperty("memberId")]
        public Guid MemberId { get; set; }
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

    public class OrganizationsRelationshipsQueryOkResponseContent
    {
        [JsonProperty("result")]
        public Relationship Result { get; set; }
    }

    public class Relationship
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}