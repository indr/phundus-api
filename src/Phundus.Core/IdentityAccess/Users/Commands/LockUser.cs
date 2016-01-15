namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;
    using Services;

    public class LockUser
    {
        public LockUser(InitiatorGuid initiatorId, UserGuid userGuid)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userGuid == null) throw new ArgumentNullException("userGuid");

            InitiatorGuid = initiatorId;
            UserGuid = userGuid;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
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
            var initiator = _userInRole.Admin(command.InitiatorGuid);

            var user = _userRepository.GetById(command.UserGuid);
            user.Account.Lock(initiator);
        }
    }
}