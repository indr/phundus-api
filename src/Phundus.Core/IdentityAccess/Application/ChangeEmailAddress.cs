namespace Phundus.IdentityAccess.Application
{
    using System;
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Users.Exceptions;
    using Users.Repositories;

    public class ChangeEmailAddress
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
        private readonly IUserRepository _userRepository;

        public ChangeEmailAddressHandler(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public void Handle(ChangeEmailAddress command)
        {
            var user = _userRepository.GetById(command.UserId);

            var emailAddress = command.NewEmailAddress.ToLower(CultureInfo.CurrentCulture).Trim();
            if (_userRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            user.ChangeEmailAddress(command.UserId, command.Password, command.NewEmailAddress);
        }
    }
}