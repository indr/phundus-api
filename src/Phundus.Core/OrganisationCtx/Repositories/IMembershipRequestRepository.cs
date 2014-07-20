namespace Phundus.Core.OrganisationCtx.Repositories
{
    using Core.Repositories;
    using DomainModel;

    public interface IMembershipRequestRepository : IRepository<MembershipRequest>
    {
        int NextIdentity();
    }
}