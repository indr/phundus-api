namespace Phundus.IdentityAccess.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Infrastructure.Persistence;
    using Model.Organizations;
    using NHibernate.Linq;
    using Organizations.Model;

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

        public Membership Find(Guid organizationId, Guid userId)
        {
            return Entities.SingleOrDefault(p => p.Organization.Id.Id == organizationId && p.UserId.Id == userId);
        }
    }
}