namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using NHibernate.Linq;

    public class NhMembershipRepository : NhRepositoryBase<Membership>, IMembershipRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<Membership> ByMemberId(int memberId)
        {
            return Entities.Where(p => p.UserId == memberId).ToFuture();
        }

        public IEnumerable<Membership> ByOrganizationId(int organizationId)
        {
            return Entities.Where(p => p.Organization.Id == organizationId).ToFuture();
        }

        public IEnumerable<Membership> GetByOrganizationId(Guid organizationId)
        {
            return Entities.Where(p => p.Organization.Guid == organizationId).ToFuture();
        }
    }
}