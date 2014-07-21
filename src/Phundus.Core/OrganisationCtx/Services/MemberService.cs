namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;
    using IdentityAndAccessCtx.Queries;

    public class MemberService : IMemberService
    {
        public IUserQueries UserQueries { get; set; }

        public Member MemberFrom(int id)
        {
            return new UserRepositoryAdapter(UserQueries).ToMember(id);
        }
    }
}