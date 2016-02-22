namespace Phundus.Integration.IdentityAccess
{
    using System;

    public interface IOrganization
    {
        Guid OrganizationId { get; }
        DateTime EstablishedAtUtc { get; }
        string Name { get; }
        string Url { get; }
        string PostAddress { get; }
        string PhoneNumber { get; }
        string EmailAddress { get; }
        string Website { get; }
        string Startpage { get; }
        string DocumentTemplate { get; }
        bool PublicRental { get; }
        string Plan { get; }
    }
}