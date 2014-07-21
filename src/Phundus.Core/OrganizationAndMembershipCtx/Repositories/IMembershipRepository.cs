namespace Phundus.Core.OrganizationAndMembershipCtx.Repositories
{
    using System;
    using Infrastructure;
    using Model;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();
    }
}