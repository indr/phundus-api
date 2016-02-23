namespace Phundus.IdentityAccess.Organizations.Repositories
{
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IMembershipRequestRepository : IRepository<MembershipApplication>
    {
        MembershipApplication GetById(MembershipApplicationId id);
    }
}