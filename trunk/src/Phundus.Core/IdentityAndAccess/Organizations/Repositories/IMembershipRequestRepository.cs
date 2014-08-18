namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRequestRepository : IRepository<MembershipApplication>
    {
        Guid NextIdentity();
        IEnumerable<MembershipApplication> PendingByOrganization(int organizationId);
        MembershipApplication ById(object id);
        MembershipApplication GetById(object id);
    }
}