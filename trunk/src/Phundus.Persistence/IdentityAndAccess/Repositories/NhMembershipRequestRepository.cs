namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.IdentityAndAccess.Organizations;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using NHibernate.Linq;

    public class NhMembershipRequestRepository : NhRepositoryBase<MembershipApplication>, IMembershipRequestRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public MembershipApplication GetById(Guid id)
        {
            var result = FindById(id);
            if (result == null)
                throw new MembershipApplicationNotFoundException(id);
            return result;
        }

        public IEnumerable<MembershipApplication> PendingByOrganization(int organizationId)
        {
            return Entities.Where(p => p.OrganizationId == organizationId)
                .Where(p => (p.ApprovalDate == null) || (p.RejectDate == null))
                .ToFuture();
        }
    }
}