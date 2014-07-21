namespace Phundus.Core.OrganizationAndMembershipCtx.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRequestRepository : IRepository<MembershipRequest>
    {
        Guid NextIdentity();
        IEnumerable<MembershipRequest> ByOrganization(int organizationId);
    }
}