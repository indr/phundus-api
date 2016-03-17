namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Users;

    public class ChangeUserAddress : ICommand
    {
        public ChangeUserAddress(InitiatorId initiatorId, UserId userId, string firstName, string lastName,
            string street, string postcode, string city, string phoneNumber)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (userId == null) throw new ArgumentNullException("userId");
            InitiatorId = initiatorId;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Postcode = postcode;
            City = city;
            PhoneNumber = phoneNumber;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public UserId UserId { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Street { get; protected set; }
        public string Postcode { get; protected set; }
        public string City { get; protected set; }
        public string PhoneNumber { get; protected set; }
    }

    public class ChangeUserAddressHandler : IHandleCommand<ChangeUserAddress>
    {
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUserRepository _userRepository;

        public ChangeUserAddressHandler(IUserInRoleService userInRoleService, IUserRepository userRepository)
        {
            _userInRoleService = userInRoleService;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ChangeUserAddress command)
        {
            if (!Equals(command.InitiatorId, command.UserId))
                throw new AuthorizationException();

            var initiator = _userInRoleService.Initiator(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);

            user.ChangeAddress(initiator, command.FirstName, command.LastName, command.Street, command.Postcode,
                command.City, command.PhoneNumber);

            _userRepository.Save(user);
        }
    }
}