namespace Phundus.IdentityAccess.Users.Services
{
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Users.Repositories;
    using Model;

    public interface IUserInRole
    {
        User Admin(UserId userId);
    }

    public class UserInRole : IUserInRole
    {
        private readonly IUserRepository _userRepository;

        public UserInRole(IUserRepository userRepository)
        {
            AssertionConcern.AssertArgumentNotNull(userRepository, "UserRepository must be provided.");

            _userRepository = userRepository;
        }

        public User Admin(UserId userId)
        {
            var user = _userRepository.GetById(userId);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return user;
        }
    }
}