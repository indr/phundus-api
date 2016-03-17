namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Users;
    using Users.Exceptions;

    public class ChangeEmailAddress : ICommand
    {
        public ChangeEmailAddress(InitiatorId initiatorId, UserId userId, string password, string newEmailAddress)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userId == null) throw new ArgumentNullException("userId");
            if (password == null) throw new ArgumentNullException("password");
            if (newEmailAddress == null) throw new ArgumentNullException("newEmailAddress");
            InitiatorId = initiatorId;
            UserId = userId;
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
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUserRepository _userRepository;

        public ChangeEmailAddressHandler(IUserInRoleService userInRoleService, IUserRepository userRepository)
        {
            _userInRoleService = userInRoleService;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ChangeEmailAddress command)
        {
            if (!Equals(command.InitiatorId, command.UserId))
                throw new AuthorizationException();

            var emailAddress = command.NewEmailAddress.ToLowerInvariant().Trim();
            if (_userRepository.FindByEmailAddress(emailAddress) != null)
                throw new EmailAlreadyTakenException();

            var initiator = _userInRoleService.Initiator(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);

            user.ChangeEmailAddress(initiator, command.Password, command.NewEmailAddress);
        }
    }
}