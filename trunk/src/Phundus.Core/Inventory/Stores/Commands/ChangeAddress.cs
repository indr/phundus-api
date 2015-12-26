namespace Phundus.Core.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Model;
    using Repositories;

    public class ChangeAddress
    {
        public int InitatorId { get; set; }
        public Guid StoreId { get; set; }
        public string Address { get; set; }
    }

    public class ChangeAddressHandler : IHandleCommand<ChangeAddress>
    {
        private readonly IStoreRepository _storeRepository;

        public ChangeAddressHandler(IStoreRepository storeRepository)
        {
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");

            _storeRepository = storeRepository;
        }

        public void Handle(ChangeAddress command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));

            if (!store.Owner.OwnerId.Equals(new OwnerId(command.InitatorId)))
                throw new AuthorizationException();

            store.ChangeAddress(command.Address);
        }
    }
}