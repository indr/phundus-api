namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using Users.Repositories;

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
        private readonly IInitiatorService _initiatorService;
        private readonly IUserRepository _userRepository;

        public ChangeUserAddressHandler(IInitiatorService initiatorService, IUserRepository userRepository)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (userRepository == null) throw new ArgumentNullException("userRepository");

            _initiatorService = initiatorService;
            _userRepository = userRepository;
        }

        public void Handle(ChangeUserAddress command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var user = _userRepository.GetByGuid(command.UserId);

            user.ChangeAddress(initiator, command.FirstName, command.LastName, command.Street, command.Postcode,
                command.City, command.PhoneNumber);

            _userRepository.Save(user);
        }
    }
}