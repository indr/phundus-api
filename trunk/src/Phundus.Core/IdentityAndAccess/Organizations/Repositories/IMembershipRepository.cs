namespace Phundus.Core.IdentityAndAccess.Organizations.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();
        
        IEnumerable<Membership> ByMemberId(int memberId);
        IEnumerable<Membership> ByOrganizationId(int organizationId);
    }
}