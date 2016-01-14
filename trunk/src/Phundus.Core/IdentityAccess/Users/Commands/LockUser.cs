namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;
    using Services;

    public class LockUser
    {
        public LockUser(UserId currentUserId, UserGuid userGuid)
        {
            if (currentUserId == null) throw new ArgumentNullException("currentUserId");
            if (userGuid == null) throw new ArgumentNullException("userGuid");

            InitiatorId = currentUserId;
            UserGuid = userGuid;
        }

        public UserId InitiatorId { get; protected set; }
        public UserGuid UserGuid { get; protected set; }
    }

    public class LockUserHandler : IHandleCommand<LockUser>
    {
        private readonly IUserInRole _userInRole;
        private readonly IUserRepository _userRepository;

        public LockUserHandler(IUserInRole userInRole, IUserRepository userRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _userInRole = userInRole;
            _userRepository = userRepository;
        }

        public void Handle(LockUser command)
        {
            var initiator = _userInRole.Admin(command.InitiatorId);

            var user = _userRepository.GetById(command.UserGuid);
            user.Account.Lock(initiator);
        }
    }
}