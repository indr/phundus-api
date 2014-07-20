namespace Phundus.Core.OrganisationCtx.Repositories
{
    using DomainModel;

    public interface IMembershipRequestRepository
    {
        int NextIdentity();
        void Add(MembershipRequest entity);
    }
}