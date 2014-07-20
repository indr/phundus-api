namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;
    using IdentityAndAccessCtx.Repositories;

    public class MemberService : IMemberService
    {
        public IUserRepository Users { get; set; }

        public Member MemberFrom(int id)
        {
            return new UserRepositoryAdapter(Users).ToMember(id);
        }
    }
}