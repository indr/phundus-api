namespace Phundus.Core.OrganisationCtx.Repositories
{
    using System;
    using System.Collections.Generic;
    using DomainModel;
    using Phundus.Infrastructure;

    public interface IMembershipRequestRepository : IRepository<MembershipRequest>
    {
        Guid NextIdentity();
        IEnumerable<MembershipRequest> ByOrganization(int organizationId);
    }
}