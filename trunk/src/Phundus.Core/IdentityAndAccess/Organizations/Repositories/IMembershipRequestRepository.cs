namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRequestRepository : IRepository<MembershipApplication>
    {
        Guid NextIdentity();
        MembershipApplication GetById(Guid id);

        IEnumerable<MembershipApplication> PendingByOrganization(int organizationId);
    }
}