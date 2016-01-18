namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;

    public class MembershipApplicationViewRow : IMembershipApplication
    {
        public virtual Guid ApplicationId { get; protected set; }
        public virtual Guid OrganizationId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual string CustomMemberNumber { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string EmailAddress { get; protected set; }
        public virtual DateTime RequestedAtUtc { get; protected set; }
        public virtual DateTime? ApprovedAtUtc { get; protected set; }
        public virtual DateTime? RejectedAtUtc { get; protected set; }
    }
}