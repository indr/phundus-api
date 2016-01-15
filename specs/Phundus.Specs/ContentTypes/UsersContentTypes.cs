namespace Phundus.Specs.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class UsersPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("organizationId")]
        public Guid? OrganizationId { get; set; }
    }

    public class UsersPostOkResponseContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }

    public class UsersGetOkResponseContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

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
}