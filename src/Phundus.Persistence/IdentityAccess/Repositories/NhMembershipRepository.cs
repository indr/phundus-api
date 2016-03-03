namespace Phundus.Persistence.IdentityAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;
    using Phundus.IdentityAccess.Model.Organizations;
    using Phundus.IdentityAccess.Organizations.Model;

    public class NhMembershipRepository : NhRepositoryBase<Membership>, IMembershipRepository
    {
        public Guid NextIdentity()
        {
            return Guid.NewGuid();
        }

        public IEnumerable<Membership> FindByOrganizationId(Guid organizationId)
        {
            return Entities.Where(p => p.Organization.Id.Id == organizationId).ToFuture();
        }

        public IEnumerable<Membership> FindByUserId(Guid userId)
        {
            return Entities.Where(p => p.UserId.Id == userId).ToFuture();
        }
    }
}