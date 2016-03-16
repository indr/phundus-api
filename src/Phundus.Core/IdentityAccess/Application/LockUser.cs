namespace Phundus.IdentityAccess.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Users;
    using Phundus.Authorization;
    using Resources;
    using Users.Services;

    public class LockUser : ICommand
    {
        public LockUser(InitiatorId initiatorId, UserId userId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userId == null) throw new ArgumentNullException("userId");

            InitiatorId = initiatorId;
            UserId = userId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
    }

    public class LockUserHandler : IHandleCommand<LockUser>
    {        
        private readonly IUserInRole _userInRole;
        private readonly IUserRepository _userRepository;

        public LockUserHandler(IUserInRole userInRole, IUserRepository userRepository)
        {                        
            _userInRole = userInRole;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(LockUser command)
        {
            var admin = _userInRole.Admin(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);

            user.Lock(admin);
        }
    }
}