namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;
    using Services;

    public class ChangeUserRole
    {
        public ChangeUserRole(UserId initiatorId, UserGuid userGuid, UserRole userRole)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userGuid == null) throw new ArgumentNullException("userGuid");
            InitiatorId = initiatorId;
            UserGuid = userGuid;
            UserRole = userRole;
        }

        public UserId InitiatorId { get; protected set; }
        public UserGuid UserGuid { get; protected set; }
        public UserRole UserRole { get; protected set; }
    }

    public class ChangeUserRoleHandler : IHandleCommand<ChangeUserRole>
    {
        private readonly IUserInRole _userInRole;
        private readonly IUserRepository _userRepository;

        public ChangeUserRoleHandler(IUserInRole userInRole, IUserRepository userRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _userInRole = userInRole;
            _userRepository = userRepository;
        }

        public void Handle(ChangeUserRole command)
        {
            var initiator = _userInRole.Admin(command.InitiatorId);

            var user = _userRepository.GetById(command.UserGuid);
            user.ChangeRole(initiator, command.UserRole);
        }
    }
}