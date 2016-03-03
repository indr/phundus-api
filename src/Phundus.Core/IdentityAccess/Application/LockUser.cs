namespace Phundus.IdentityAccess.Application
{
    using System;
    using Authorization;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Users;
    using Phundus.Authorization;

    public class LockUser
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
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;
        private readonly IUserRepository _userRepository;

        public LockUserHandler(IAuthorize authorize, IInitiatorService initiatorService, IUserRepository userRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _authorize = authorize;
            _initiatorService = initiatorService;
            _userRepository = userRepository;
        }

        public void Handle(LockUser command)
        {
            var initiator = _initiatorService.GetById(command.InitiatorId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Users);

            var user = _userRepository.GetById(command.UserId);

            user.Lock(initiator);
        }
    }
}