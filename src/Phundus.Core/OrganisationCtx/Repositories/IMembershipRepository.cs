namespace Phundus.Core.OrganisationCtx.Repositories
{
    using System;
    using DomainModel;
    using Phundus.Infrastructure;

    public interface IMembershipRepository : IRepository<Membership>
    {
        Guid NextIdentity();
    }
}