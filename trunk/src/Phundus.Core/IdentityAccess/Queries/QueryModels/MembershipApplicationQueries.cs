namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Cqrs;
    using NHibernate;

    public class MembershipApplicationQueries : NHibernateReadModelBase<MembershipApplicationViewRow>, IMembershipApplicationQueries
    {
        public MembershipApplicationQueries(Func<ISession> sessionFactory) : base(sessionFactory)
        {
        }

        public IList<IMembershipApplication> FindPending(CurrentUserId currentUserId, OrganizationId organizationId)
        {
            // TODO: Access filtering
            return QueryOver()
                .Where(p => p.OrganizationId == organizationId.Id && p.ApprovedAtUtc == null && p.RejectedAtUtc == null)
                .List<IMembershipApplication>();
        }
    }
}