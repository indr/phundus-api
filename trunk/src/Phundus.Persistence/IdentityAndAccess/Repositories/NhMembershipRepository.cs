namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IdentityAccess.Organizations.Model;
    using IdentityAccess.Organizations.Repositories;
    using NHibernate.Linq;

    public class NhMembershipRepository : NhRepositoryBase<Membership>, IMembershipRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<Membership> ByMemberId(Guid memberId)
        {
            return Entities.Where(p => p.UserGuid.Id == memberId).ToFuture();
        }

        public IEnumerable<Membership> GetByOrganizationId(Guid organizationId)
        {
            return Entities.Where(p => p.Organization.Id == organizationId).ToFuture();
        }
    }
}