﻿namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;
    using Services;

    public class UnlockUser
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

    public class UnlockHandler : IHandleCommand<UnlockUser>
    {
        private readonly IUserInRole _userInRole;
        private readonly IUserRepository _userRepository;

        public UnlockHandler(IUserInRole userInRole, IUserRepository userRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _userInRole = userInRole;
            _userRepository = userRepository;
        }

        public void Handle(UnlockUser command)
        {
            var initiator = _userInRole.Admin(command.InitiatorId);

            var user = _userRepository.GetByGuid(command.UserId);
            user.Account.Unlock(initiator);
        }
    }
}