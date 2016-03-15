﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Users;
    using Resources;
    using Users.Services;

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
        private readonly IUserInRole _userInRole;
        private readonly IUserRepository _userRepository;

        public ChangeUserRoleHandler(IUserInRole userInRole, IUserRepository userRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userInRole = userInRole;
            _userRepository = userRepository;
        }
        
        [Transaction]
        public void Handle(ChangeUserRole command)
        {
            var initiator = _userInRole.Admin(command.InitiatorId);

            var user = _userRepository.GetById(command.UserId);
            user.ChangeRole(initiator, command.UserRole);
        }
    }
}