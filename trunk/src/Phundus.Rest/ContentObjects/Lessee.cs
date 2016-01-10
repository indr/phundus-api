﻿namespace Phundus.Rest.ContentObjects
{
    using Newtonsoft.Json;

    public class Lessee
    {
        [JsonProperty("lesseeId")]
        public int LesseeId { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("memberNumber")]
        public string MemberNumber { get; set; }
    }
}