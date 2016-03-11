namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Querying;

    public interface IMembershipApplicationQueries
    {
        IList<MembershipApplicationData> FindPending(CurrentUserId currentUserId, OrganizationId organizationId);
    }

    public class MembershipApplicationQueries : QueryBase<MembershipApplicationData>,
        IMembershipApplicationQueries
    {
        public IList<MembershipApplicationData> FindPending(CurrentUserId currentUserId, OrganizationId organizationId)
        {
            // TODO: Access filtering
            return QueryOver()
                .Where(p => p.OrganizationId == organizationId.Id && p.ApprovedAtUtc == null && p.RejectedAtUtc == null)
                .List<MembershipApplicationData>();
        }
    }

    public class MembershipApplicationData
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