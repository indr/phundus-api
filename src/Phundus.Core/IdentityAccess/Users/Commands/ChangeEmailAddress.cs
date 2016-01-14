﻿namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Exceptions;
    using Infrastructure.Gateways;
    using Mails;
    using Repositories;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(UserGuid initiatorGuid, string password, string newEmailAddress)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            InitiatorGuid = initiatorGuid;
            Password = password;
            NewEmailAddress = newEmailAddress;
        }

        public UserGuid InitiatorGuid { get; protected set; }
        public string Password { get; protected set; }
        public string NewEmailAddress { get; protected set; }
    }

    public class ChangeEmailAddressHandler : IHandleCommand<ChangeEmailAddress>
    {
        private readonly IMailGateway _mailGateway;
        private readonly IUserRepository _userRepository;

        public ChangeEmailAddressHandler(IUserRepository userRepository, IMailGateway mailGateway)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            if (mailGateway == null) throw new ArgumentNullException("mailGateway");
            _userRepository = userRepository;
            _mailGateway = mailGateway;
        }

        public void Handle(ChangeEmailAddress command)
        {
            var user = _userRepository.GetById(command.InitiatorGuid);

            var emailAddress = command.NewEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (_userRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.ChangeEmailAddress(command.InitiatorGuid, command.Password, command.NewEmailAddress);
        }
    }
}