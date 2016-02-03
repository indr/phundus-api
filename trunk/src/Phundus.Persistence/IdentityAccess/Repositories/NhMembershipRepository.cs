namespace Phundus.Persistence.IdentityAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;

    public class NhMembershipRepository : NhRepositoryBase<Membership>, IMembershipRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<Membership> ByMemberId(Guid memberId)
        {
            return Entities.Where(p => p.UserId.Id == memberId).ToFuture();
        }

        public IEnumerable<Membership> GetByOrganizationId(Guid organizationId)
        {
            return Entities.Where(p => p.Organization.Id.Id == organizationId).ToFuture();
        }
    }
}