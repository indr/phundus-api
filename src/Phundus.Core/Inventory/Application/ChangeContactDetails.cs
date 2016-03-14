namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Stores;

    public class ChangeContactDetails : ICommand
    {
        public ChangeContactDetails(InitiatorId initiatorId, StoreId storeId, string emailAddress, string phoneNumber,
            string line1, string line2, string street, string postcode, string city)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            InitiatorId = initiatorId;
            StoreId = storeId;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Line1 = line1;
            Line2 = line2;
            Street = street;
            Postcode = postcode;
            City = city;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public StoreId StoreId { get; protected set; }
        public string EmailAddress { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string Line1 { get; protected set; }
        public string Line2 { get; protected set; }
        public string Street { get; protected set; }
        public string Postcode { get; protected set; }
        public string City { get; protected set; }
    }

    public class ChangeContactDetailsHandler : IHandleCommand<ChangeContactDetails>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUserInRole _userInRole;

        public ChangeContactDetailsHandler(IStoreRepository storeRepository, IUserInRole userInRole)
        {
            _storeRepository = storeRepository;
            _userInRole = userInRole;
        }

        [Transaction]
        public void Handle(ChangeContactDetails command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _userInRole.Manager(command.InitiatorId, store.OwnerId);

            store.ChangeContactDetails(manager, new ContactDetails(command.EmailAddress, command.PhoneNumber,
                new PostalAddress(command.Line1, command.Line2, command.Street, command.Postcode, command.City)));

            _storeRepository.Save(store);
        }
    }
}