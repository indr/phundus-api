namespace Phundus.Persistence.OrganisationCtx
{
    using System;
    using System.Collections.Generic;
    using Core.OrganizationAndMembershipCtx.Model;
    using Core.OrganizationAndMembershipCtx.Repositories;
    using NHibernate;

    public class MembershipRepository : RepositoryBase<Membership>, IMembershipRepository
    {
        public Guid NextIdentity()
        {
            throw new NotImplementedException();
        }

        public IList<Membership> ByMemberId(int memberId)
        {
            return Entities.Where(p => p.MemberId == memberId).List();
        }

        private IQueryOver<Membership, Membership> Entities { get { return Session.QueryOver<Membership>(); } }
    }

    public class MembershipRequestRepository : RepositoryBase<MembershipRequest>, IMembershipRequestRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<MembershipRequest> ByOrganization(int organizationId)
        {
            return Entities.Where(p => p.OrganizationId == organizationId).List();
        }

        private IQueryOver<MembershipRequest, MembershipRequest> Entities
        {
            get { return Session.QueryOver<MembershipRequest>(); }
        }
    }
}