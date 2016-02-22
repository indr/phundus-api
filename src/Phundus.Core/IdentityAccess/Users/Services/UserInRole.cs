namespace Phundus.IdentityAccess.Users.Services
{
    using Common;
    using Common.Domain.Model;
    using Model;
    using Repositories;

    public interface IUserInRole
    {
        Admin Admin(UserId userId);
        bool IsAdmin(UserId userId);
    }

    public class UserInRole : IUserInRole
    {
        private readonly IUserRepository _userRepository;

        public UserInRole(IUserRepository userRepository)
        {
            AssertionConcern.AssertArgumentNotNull(userRepository, "UserRepository must be provided.");

            _userRepository = userRepository;
        }

        public Admin Admin(UserId userId)
        {
            var user = _userRepository.GetByGuid(userId);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return new Admin(user.UserId, user.EmailAddress, user.FullName);
        }

        public bool IsAdmin(UserId userId)
        {
            var user = _userRepository.GetByGuid(userId);
            return user.Role == UserRole.Admin;
        }
    }
}