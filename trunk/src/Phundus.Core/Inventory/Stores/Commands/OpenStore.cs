namespace Phundus.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;
    using Services;

    public class OpenStore
    {
        public OpenStore(InitiatorId initiatorId, OwnerId ownerId, StoreId storeId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (storeId == null) throw new ArgumentNullException("storeId");

            InitiatorId = initiatorId;
            OwnerId = ownerId;
            StoreId = storeId;
        }

        public InitiatorId InitiatorId { get; private set; }
        public OwnerId OwnerId { get; private set; }
        public StoreId StoreId { get; private set; }
    }

    public class OpenStoreHandler : IHandleCommand<OpenStore>
    {
        private readonly IOwnerService _ownerService;
        private readonly IStoreRepository _storeRepository;

        public OpenStoreHandler(IStoreRepository storeRepository, IOwnerService ownerService)
        {
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerService, "OwnerService must be provided.");

            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        public void Handle(OpenStore command)
        {
            if (command.OwnerId.Id != command.InitiatorId.Id)
                throw new AuthorizationException();

            var owner = _ownerService.GetById(command.OwnerId);
            var store = new Store(command.StoreId, owner);

            _storeRepository.Add(store);

            EventPublisher.Publish(new StoreOpened());
        }
    }
}