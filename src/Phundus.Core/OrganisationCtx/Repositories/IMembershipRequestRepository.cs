namespace Phundus.Core.OrganisationCtx.Repositories
{
    #region

    using System;
    using System.Collections.Generic;
    using Core.Repositories;
    using DomainModel;

    #endregion

    public interface IMembershipRequestRepository : IRepository<MembershipRequest>
    {
        Guid NextIdentity();
        IEnumerable<MembershipRequest> ByOrganization(int organizationId);
    }
}