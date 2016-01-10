namespace Phundus.Specs.Api
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Phundus.Rest.ContentObjects;

    public class UsersGetOkResponseContent
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userGuid")]
        public Guid UserGuid { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        [JsonProperty("memberships")]
        public List<Memberships> Memberships { get; set; }

        [JsonProperty("store")]
        public Store Store { get; set; }
    }

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

    public class Store
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }

        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }
}