namespace Phundus.Persistence.OrganisationCtx
{
    using System;
    using System.Collections.Generic;
    using Core.OrganizationAndMembershipCtx.Model;
    using Core.OrganizationAndMembershipCtx.Repositories;
    using NHibernate;

    public class MembershipRepository : RepositoryBase<Membership>, IMembershipRepository
    {
        private IQueryOver<Membership, Membership> Entities
        {
            get { return Session.QueryOver<Membership>(); }
        }

        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IList<Membership> ByMemberId(int memberId)
        {
            return Entities.Where(p => p.MemberId == memberId).List();
        }

        public IList<Membership> ByOrganizationId(int organizationId)
        {
            return Entities.Where(p => p.OrganizationId == organizationId).List();
        }
    }

    public class MembershipRequestRepository : RepositoryBase<MembershipRequest>, IMembershipRequestRepository
    {
        private IQueryOver<MembershipRequest, MembershipRequest> Entities
        {
            get { return Session.QueryOver<MembershipRequest>(); }
        }

        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<MembershipRequest> PendingByOrganization(int organizationId)
        {
            return
                Entities.Where(p => p.OrganizationId == organizationId)
                    .Where(p => (p.ApprovalDate == null) || (p.RejectDate == null))
                    .List();
        }
    }
}