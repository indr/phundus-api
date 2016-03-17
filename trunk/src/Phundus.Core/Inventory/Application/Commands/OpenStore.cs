namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Collaborators;
    using Model.Stores;

    public class OpenStore : ICommand
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

        public InitiatorId InitiatorId { get; protected set; }
        public OwnerId OwnerId { get; protected set; }
        public StoreId StoreId { get; protected set; }
    }

    public class OpenStoreHandler : IHandleCommand<OpenStore>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IOwnerService _ownerService;
        private readonly IStoreRepository _storeRepository;

        public OpenStoreHandler(IStoreRepository storeRepository, ICollaboratorService collaboratorService,
            IOwnerService ownerService)
        {
            _storeRepository = storeRepository;
            _collaboratorService = collaboratorService;
            _ownerService = ownerService;
        }

        [Transaction]
        public void Handle(OpenStore command)
        {
            var owner = _ownerService.GetById(command.OwnerId);
            var manager = _collaboratorService.Manager(command.InitiatorId, owner.OwnerId);

            var store = new Store(manager, command.StoreId, owner);

            _storeRepository.Add(store);
        }
    }
}