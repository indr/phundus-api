namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRequestRepository : IRepository<MembershipRequest>
    {
        Guid NextIdentity();
        IEnumerable<MembershipRequest> PendingByOrganization(int organizationId);
    }
}