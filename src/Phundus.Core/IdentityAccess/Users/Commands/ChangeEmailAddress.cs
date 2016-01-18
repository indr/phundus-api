namespace Phundus.IdentityAccess.Users.Commands
{
    using System;
    using System.Globalization;
    using Common.Domain.Model;
    using Cqrs;
    using Exceptions;
    using Repositories;

    public class ChangeEmailAddress
    {
        public ChangeEmailAddress(InitiatorId initiatorId, string password, string newEmailAddress)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");

            InitiatorId = initiatorId;
            UserGuid = new UserGuid(initiatorId.Id);
            Password = password;
            NewEmailAddress = newEmailAddress;
        }

        public InitiatorId InitiatorId { get; protected set; }
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