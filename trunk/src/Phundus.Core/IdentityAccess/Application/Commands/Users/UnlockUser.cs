namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Users;

    public class UnlockUser : ICommand
    {
        public UnlockUser(InitiatorId initiatorId, UserId userId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userId == null) throw new ArgumentNullException("userId");
            InitiatorId = initiatorId;
            UserId = userId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
    }

    public class UnlockUserHandler : IHandleCommand<UnlockUser>
    {
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUserRepository _userRepository;

        public UnlockUserHandler(IUserInRoleService userInRoleService, IUserRepository userRepository)
        {
            _userInRoleService = userInRoleService;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(UnlockUser command)
        {
            var initiator = _userInRoleService.Admin(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);

            user.Unlock(initiator);
        }
    }
}