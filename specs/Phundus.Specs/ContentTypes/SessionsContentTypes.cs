namespace Phundus.Specs.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class SessionsPostRequestContent
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class SessionsPostOkResponseContent
    {
        [JsonProperty("memberships")]
        public List<Memberships> Memberships { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class Role
    {
        [JsonProperty("bitMask")]
        public int BitMask { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}