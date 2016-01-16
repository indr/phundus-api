namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;

    public interface IMembershipApplication
    {
        Guid ApplicationId { get; }
        Guid OrganizationId { get; }
        Guid UserId { get; }
        string CustomMemberNumber { get; }
        string FirstName { get; }
        string LastName { get; }
        string EmailAddress { get; }
        DateTime RequestedAtUtc { get; }
        DateTime? ApprovedAtUtc { get; }
        DateTime? RejectedAtUtc { get; }
    }
}