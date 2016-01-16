﻿namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Exceptions;
    using Repositories;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(InitiatorGuid initiatorGuid, string password, string newEmailAddress)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            InitiatorGuid = initiatorGuid;
            UserGuid = new UserGuid(initiatorGuid.Id);
            Password = password;
            NewEmailAddress = newEmailAddress;
        }

        public InitiatorGuid InitiatorGuid { get; protected set; }
        public UserGuid UserGuid { get; protected set; }
        public string Password { get; protected set; }
        public string NewEmailAddress { get; protected set; }
    }

    public class ChangeEmailAddressHandler : IHandleCommand<ChangeEmailAddress>
    {
        private readonly IUserRepository _userRepository;

        public ChangeEmailAddressHandler(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public void Handle(ChangeEmailAddress command)
        {
            var user = _userRepository.GetByGuid(command.UserGuid);

            var emailAddress = command.NewEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (_userRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.ChangeEmailAddress(command.UserGuid, command.Password, command.NewEmailAddress);
        }
    }
}