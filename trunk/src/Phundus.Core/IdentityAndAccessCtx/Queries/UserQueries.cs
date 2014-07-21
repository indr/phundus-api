namespace Phundus.Core.IdentityAndAccessCtx.Queries
{
    using DomainModel;
    using Repositories;

    public class UserQueries : IUserQueries
    {
        public IUserRepository UserRepository { get; set; }

        public User ById(int id)
        {
            return UserRepository.FindById(id);
        }
    }
}