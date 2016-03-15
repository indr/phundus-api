﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using System.Globalization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Users;
    using Users.Exceptions;
    using Users.Services;

    public class ChangeEmailAddress : ICommand
    {
        public ChangeEmailAddress(InitiatorId initiatorId, string password, string newEmailAddress)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");
            InitiatorId = initiatorId;
            UserId = new UserId(initiatorId.Id);
            Password = password;
            NewEmailAddress = newEmailAddress;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
        public string Password { get; protected set; }
        public string NewEmailAddress { get; protected set; }
    }

    public class ChangeEmailAddressHandler : IHandleCommand<ChangeEmailAddress>
    {
        private readonly IUserInRole _userInRole;
        private readonly IUserRepository _userRepository;

        public ChangeEmailAddressHandler(IUserInRole userInRole, IUserRepository userRepository)
        {
            _userInRole = userInRole;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ChangeEmailAddress command)
        {
            var initiator = _userInRole.GetById(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);

            var emailAddress = command.NewEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (_userRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.ChangeEmailAddress(initiator, command.Password, command.NewEmailAddress);
        }
    }
}