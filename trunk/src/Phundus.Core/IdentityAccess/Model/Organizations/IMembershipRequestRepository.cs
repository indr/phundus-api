namespace Phundus.IdentityAccess.Model.Organizations
{
    using Common.Domain.Model;
    using IdentityAccess.Organizations.Model;

    public interface IMembershipRequestRepository : IRepository<MembershipApplication>
    {
        MembershipApplication GetById(MembershipApplicationId id);
    }
}