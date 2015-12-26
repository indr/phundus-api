namespace Phundus.Core.Inventory.Stores.Commands
{
    using Common;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;
    using Services;

    public class OpenStore
    {
        public int InitiatorId { get; set; }
        public string StoreId { get; set; }
        public int UserId { get; set; }
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
            if (command.UserId != command.InitiatorId)
                throw new AuthorizationException();

            var owner = _ownerService.GetByUserId(command.UserId);
            var store = owner.OpenStore();

            _storeRepository.Add(store);

            EventPublisher.Publish(new StoreOpened());
        }
    }
}