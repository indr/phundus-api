namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class OrganizationsPostRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class OrganizationsPostOkResponseContent
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }
    }

    public class Organization
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("startpage")]
        public string Startpage { get; set; }

        [JsonProperty("contactDetails")]
        public ContactDetails ContactDetails { get; set; }

        public override string ToString()
        {
            return String.Format("[Id={0}, Name={1}]",
                new object[] { OrganizationId.ToString("D"), Name });
        }
    }

    public class ContactDetails
    {
        [JsonProperty("postAddress")]
        public string PostAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }

    public class OrganizationsSettings
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("publicRental")]
        public bool PublicRental { get; set; }
    }
}