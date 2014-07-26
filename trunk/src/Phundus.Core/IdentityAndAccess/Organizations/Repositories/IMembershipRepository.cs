namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();

        IList<Membership> ByMemberId(int memberId);
        IList<Membership> ByOrganizationId(int organizationId);
    }
}