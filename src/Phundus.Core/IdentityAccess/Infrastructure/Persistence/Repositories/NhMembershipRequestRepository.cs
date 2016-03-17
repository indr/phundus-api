namespace Phundus.IdentityAccess.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Model.Organizations;
    using NHibernate.Linq;
    using Organizations.Model;

    public class NhMembershipRequestRepository : NhRepositoryBase<MembershipApplication>, IMembershipRequestRepository
    {
        public MembershipApplication GetById(MembershipApplicationId id)
        {
            MembershipApplication result = FindById(id);
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