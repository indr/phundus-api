namespace Phundus.Core.OrganizationAndMembershipCtx.Repositories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();

        IList<Membership> ByMemberId(int memberId);
    }
}