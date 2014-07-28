namespace Phundus.Persistence.OrganisationCtx
{
    using System;
    using System.Collections.Generic;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;
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
            return Entities.Where(p => p.UserId == memberId).List();
        }

        public IList<Membership> ByOrganizationId(int organizationId)
        {
            return Entities.Where(p => p.Organization.Id == organizationId).List();
        }
    }

    public class MembershipRequestRepository : RepositoryBase<MembershipApplication>, IMembershipRequestRepository
    {
        private IQueryOver<MembershipApplication, MembershipApplication> Entities
        {
            get { return Session.QueryOver<MembershipApplication>(); }
        }

        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<MembershipApplication> PendingByOrganization(int organizationId)
        {
            return
                Entities.Where(p => p.OrganizationId == organizationId)
                    .Where(p => (p.ApprovalDate == null) || (p.RejectDate == null))
                    .List();
        }
    }
}