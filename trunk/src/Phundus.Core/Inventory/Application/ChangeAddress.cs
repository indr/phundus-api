namespace Phundus.Inventory.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Inventory.Model;
    using Stores.Repositories;

    public class ChangeAddress
    {
        public ChangeAddress(InitiatorId initiatorId, StoreId storeId, string address)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (address == null) throw new ArgumentNullException("address");

            InitatorId = initiatorId;
            StoreId = storeId;
            Address = address;
        }

        public InitiatorId InitatorId { get; protected set; }
        public StoreId StoreId { get; protected set; }
        public string Address { get; protected set; }
    }

    public class ChangeAddressHandler : IHandleCommand<ChangeAddress>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUserInRole _userInRole;

        public ChangeAddressHandler(IStoreRepository storeRepository, IUserInRole userInRole)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (userInRole == null) throw new ArgumentNullException("userInRole");

            _storeRepository = storeRepository;
            _userInRole = userInRole;
        }

        public void Handle(ChangeAddress command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _userInRole.Manager(command.InitatorId, store.Owner.OwnerId);

            store.ChangeAddress(manager, command.Address);

            _storeRepository.Save(store);
        }
    }
}