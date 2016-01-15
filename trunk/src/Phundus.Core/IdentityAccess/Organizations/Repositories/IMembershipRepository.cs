namespace Phundus.IdentityAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();

        IEnumerable<Membership> ByMemberId(Guid memberId);
        IEnumerable<Membership> GetByOrganizationId(Guid organizationId);
    }
}