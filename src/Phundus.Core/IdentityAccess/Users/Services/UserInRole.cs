namespace Phundus.IdentityAccess.Users.Services
{
    using Common;
    using Common.Domain.Model;
    using Model;
    using Repositories;

    public interface IUserInRole
    {
        User Admin(UserGuid userGuid);
    }

    public class UserInRole : IUserInRole
    {
        private readonly IUserRepository _userRepository;

        public UserInRole(IUserRepository userRepository)
        {
            AssertionConcern.AssertArgumentNotNull(userRepository, "UserRepository must be provided.");

            _userRepository = userRepository;
        }

        public User Admin(UserGuid userGuid)
        {
            var user = _userRepository.GetByGuid(userGuid);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return user;
        }
    }
}