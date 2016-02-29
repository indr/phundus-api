namespace Phundus.Integration.IdentityAccess
{
    using System;

    public interface IOrganization
    {
        Guid OrganizationId { get; }
        DateTime EstablishedAtUtc { get; }
        string Name { get; }
        string Url { get; }
        string PostalAddress { get; }
        string Line1 { get; }
        string Line2 { get; }
        string Street { get; }
        string Postcode { get; }
        string City { get; }
        string PhoneNumber { get; }
        string EmailAddress { get; }
        string Website { get; }
        string Startpage { get; }
        string DocumentTemplate { get; }
        bool PublicRental { get; }
        string Plan { get; }
    }
}