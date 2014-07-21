namespace Phundus.Core.OrganizationAndMembershipCtx.Services
{
    using IdentityAndAccessCtx.Queries;
    using Model;

    public class MemberService : IMemberService
    {
        public IUserQueries UserQueries { get; set; }

        public Member MemberFrom(int id)
        {
            return new UserRepositoryAdapter(UserQueries).ToMember(id);
        }
    }
}