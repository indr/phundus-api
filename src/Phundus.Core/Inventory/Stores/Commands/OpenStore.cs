namespace Phundus.Inventory.Stores.Commands
{
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;
    using Services;

    public class OpenStore
    {
        public OpenStore(UserId initiatorId, UserId ownerId, StoreId storeId)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "InitiatorId must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeId, "StoreId must be provided.");

            InitiatorId = initiatorId;
            OwnerId = ownerId;
            StoreId = storeId;
        }

        public UserId InitiatorId { get; private set; }
        public UserId OwnerId { get; private set; }
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

            var owner = _ownerService.GetByUserId(command.OwnerId);
            var store = owner.OpenStore(command.StoreId);

            _storeRepository.Add(store);

            EventPublisher.Publish(new StoreOpened());
        }
    }
}