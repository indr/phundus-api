namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Cqrs;

    public interface IMembershipApplicationQueries
    {
        IList<IMembershipApplication> FindPending(CurrentUserId currentUserId, OrganizationId organizationId);
    }

    public class MembershipApplicationQueries : ProjectionBase<MembershipApplicationData>,
        IMembershipApplicationQueries
    {
        public IList<IMembershipApplication> FindPending(CurrentUserId currentUserId, OrganizationId organizationId)
        {
            // TODO: Access filtering
            return QueryOver()
                .Where(p => p.OrganizationId == organizationId.Id && p.ApprovedAtUtc == null && p.RejectedAtUtc == null)
                .List<IMembershipApplication>();
        }
    }

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

    public class MembershipApplicationData : IMembershipApplication
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