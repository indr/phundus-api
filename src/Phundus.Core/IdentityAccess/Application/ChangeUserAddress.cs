namespace Phundus.IdentityAccess.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Users;
    using Phundus.Authorization;
    using Users.Services;

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
        private readonly IAuthorize _authorize;
        private readonly IUserInRole _initiatorService;
        private readonly IUserRepository _userRepository;

        public ChangeUserAddressHandler(IAuthorize authorize, IUserInRole initiatorService, IUserRepository userRepository)
        {
            _authorize = authorize;
            _initiatorService = initiatorService;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(ChangeUserAddress command)
        {
            _authorize.Enforce(command.InitiatorId, Manage.User(command.UserId));

            var initiator = _initiatorService.GetById(command.InitiatorId);
            var user = _userRepository.GetById(command.UserId);

            user.ChangeAddress(initiator, command.FirstName, command.LastName, command.Street, command.Postcode,
                command.City, command.PhoneNumber);

            _userRepository.Save(user);
        }
    }
}