namespace Phundus.Persistence.IdentityAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;
    using Phundus.IdentityAccess.Organizations.Exceptions;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;

    public class NhMembershipRequestRepository : NhRepositoryBase<MembershipApplication>, IMembershipRequestRepository
    {
        public MembershipApplication GetById(Guid id)
        {
            var result = FindById(id);
            if (result == null)
                throw new MembershipApplicationNotFoundException(id);
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