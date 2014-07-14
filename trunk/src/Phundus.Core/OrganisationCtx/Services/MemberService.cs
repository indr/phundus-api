namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;

    public class MemberService : IMemberService
    {
        public UserRepositoryAdapter UserRepositoryAdapter { get; set; }

        public Member MemberFrom(int id)
        {
            return UserRepositoryAdapter.ToMember(id);
        }
    }
}