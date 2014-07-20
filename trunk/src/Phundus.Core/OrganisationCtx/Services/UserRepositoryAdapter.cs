namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;
    using IdentityAndAccessCtx.DomainModel;
    using IdentityAndAccessCtx.Repositories;

    public class UserRepositoryAdapter
    {
        private readonly IUserRepository _users;

        public UserRepositoryAdapter(IUserRepository users)
        {
            _users = users;
        }

        public Member ToMember(int id)
        {
            User user = _users.FindById(id);
            if (user == null)
                return null;

            return new Member(user.Id, user.FirstName, user.LastName);
        }
    }
}