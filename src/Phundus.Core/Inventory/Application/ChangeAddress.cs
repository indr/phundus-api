namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Stores;

    public class ChangeAddress : ICommand
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
            _storeRepository = storeRepository;
            _userInRole = userInRole;
        }

        [Transaction]
        public void Handle(ChangeAddress command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _userInRole.Manager(command.InitatorId, store.OwnerId);

            store.ChangeAddress(manager, command.Address);

            _storeRepository.Save(store);
        }
    }
}