namespace Phundus.Persistence.IdentityAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using NHibernate.Linq;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;

    public class NhMembershipRequestRepository : NhRepositoryBase<MembershipApplication>, IMembershipRequestRepository
    {
        public MembershipApplication GetById(MembershipApplicationId id)
        {
            var result = FindById(id);
            if (result == null)
                throw new NotFoundException("Membership application {0} not found.", id);
            return result;
        }

        public IEnumerable<MembershipApplication> PendingByOrganization(Guid organizationId)
        {
            return Entities.Where(p => p.OrganizationId.Id == organizationId)
                .Where(p => (p.ApprovedAtUtc == null) || (p.RejectedAtUtc == null))
                .ToFuture();
        }
    }
}