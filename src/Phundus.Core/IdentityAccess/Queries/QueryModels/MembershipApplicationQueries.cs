namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Cqrs;

    public class MembershipApplicationQueries : NHibernateReadModelBase<MembershipApplicationViewRow>, IMembershipApplicationQueries
    {
        public IList<IMembershipApplication> FindPending(CurrentUserId currentUserId, OrganizationId organizationId)
        {
            // TODO: Access filtering
            return QueryOver()
                .Where(p => p.OrganizationId == organizationId.Id && p.ApprovedAtUtc == null && p.RejectedAtUtc == null)
                .List<IMembershipApplication>();
        }
    }
}