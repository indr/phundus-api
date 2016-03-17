namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Users;
    using Users.Services;

    public class ApproveUser : ICommand
    {
        public ApproveUser(InitiatorId initiatorId, UserId userId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userId == null) throw new ArgumentNullException("userId");
            InitiatorId = initiatorId;
            UserId = userId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
    }

    public class ApproveUserHandler : IHandleCommand<ApproveUser>
    {
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUserRepository _userRepository;

        public ApproveUserHandler(IUserInRoleService userInRoleService, IUserRepository userRepository)
        {
            if (userInRoleService == null) throw new ArgumentNullException("userInRoleService");
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userInRoleService = userInRoleService;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ApproveUser command)
        {
            var initiator = _userInRoleService.Admin(command.InitiatorId);

            var user = _userRepository.GetById(command.UserId);
            user.Approve(initiator);
        }
    }
}