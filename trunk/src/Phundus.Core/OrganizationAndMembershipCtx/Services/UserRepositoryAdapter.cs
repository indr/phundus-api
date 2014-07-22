namespace Phundus.Core.OrganizationAndMembershipCtx.Services
{
    using IdentityAndAccessCtx.Queries;
    using Model;

    public class UserRepositoryAdapter
    {
        private readonly IUserQueries _users;

        public UserRepositoryAdapter(IUserQueries users)
        {
            _users = users;
        }

        public Member ToMember(int id)
        {
            UserDto user = _users.ById(id);
            if (user == null)
                return null;

            return new Member(user.Id, user.FirstName, user.LastName);
        }
    }
}