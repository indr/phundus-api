namespace Phundus.IdentityAccess.Model.Organizations
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using IdentityAccess.Organizations.Model;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();

        IEnumerable<Membership> FindByOrganizationId(Guid organizationId);
        IEnumerable<Membership> FindByUserId(Guid userId);
    }
}