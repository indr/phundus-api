namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class AdminUser
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }

        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("signedUpAtUtc")]
        public DateTime SignedUpAtUtc { get; set; }

        [JsonProperty("lastLogInAtUtc")]
        public DateTime? LastLogInAtUtc { get; set; }
    }
}