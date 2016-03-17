namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Users;

    public class ChangeUserRole : ICommand
    {
        public ChangeUserRole(InitiatorId initiatorId, UserId userId, UserRole userRole)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userId == null) throw new ArgumentNullException("userId");
            InitiatorId = initiatorId;
            UserId = userId;
            UserRole = userRole;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
        public UserRole UserRole { get; protected set; }
    }

    public class ChangeUserRoleHandler : IHandleCommand<ChangeUserRole>
    {
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUserRepository _userRepository;

        public ChangeUserRoleHandler(IUserInRoleService userInRoleService, IUserRepository userRepository)
        {            
            _userInRoleService = userInRoleService;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ChangeUserRole command)
        {
            var admin = _userInRoleService.Admin(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);
            
            user.ChangeRole(admin, command.UserRole);
        }
    }
}