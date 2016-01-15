namespace Phundus.IdentityAccess.Users.Services
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Users.Repositories;
    using Model;

    public interface IUserInRole
    {
        [Obsolete]
        User Admin(UserId userId);

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

        public User Admin(UserId userId)
        {
            var user = _userRepository.GetById(userId);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return user;
        }

        public User Admin(UserGuid userGuid)
        {
            var user = _userRepository.GetById(userGuid);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return user;
        }
    }
}