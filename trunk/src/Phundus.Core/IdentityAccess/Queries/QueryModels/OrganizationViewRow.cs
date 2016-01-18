namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;
    using Integration.IdentityAccess;

    public class OrganizationViewRow : IOrganization
    {
        public Guid OrganizationId { get; set; }
        public DateTime EstablishedAtUtc { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public string PostAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }

        public string Startpage { get; set; }
        public string DocumentTemplate { get; set; }
    }
}