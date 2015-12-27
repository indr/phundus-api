namespace Phundus.Core.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Model;
    using Repositories;
    using Services;

    public class ChangeAddress
    {
        public int InitatorId { get; set; }
        public Guid StoreId { get; set; }
        public string Address { get; set; }
    }

    public class ChangeAddressHandler : IHandleCommand<ChangeAddress>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IOwnerService _ownerService;

        public ChangeAddressHandler(IStoreRepository storeRepository, IOwnerService ownerService)
        {
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerService, "OwnerService must be provided.");

            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        public void Handle(ChangeAddress command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));
            var owner = _ownerService.GetByUserId(command.InitatorId);
            
            if (!Equals(store.Owner, owner))
                throw new AuthorizationException();

            store.ChangeAddress(command.Address);
        }
    }
}