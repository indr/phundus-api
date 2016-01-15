namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class AdminUsersPatchRequestContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("isApproved")]
        public bool? IsApproved { get; set; }

        [JsonProperty("isLocked")]
        public bool? IsLocked { get; set; }

        [JsonProperty("isAdmin")]
        public bool? IsAdmin { get; set; }
    }
}