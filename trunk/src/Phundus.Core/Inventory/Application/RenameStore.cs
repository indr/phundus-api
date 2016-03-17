namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Collaborators;
    using Model.Stores;

    public class RenameStore : ICommand
    {
        public RenameStore(InitiatorId initiatorId, StoreId storeId, string name)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (name == null) throw new ArgumentNullException("name");

            InitiatorId = initiatorId;
            StoreId = storeId;
            Name = name;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public StoreId StoreId { get; protected set; }
        public string Name { get; protected set; }
    }

    public class RenameStoreHandler : IHandleCommand<RenameStore>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IStoreRepository _storeRepository;

        public RenameStoreHandler(IStoreRepository storeRepository, ICollaboratorService collaboratorService)
        {            
            _storeRepository = storeRepository;
            _collaboratorService = collaboratorService;
        }

        [Transaction]
        public void Handle(RenameStore command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _collaboratorService.Manager(command.InitiatorId, store.OwnerId);

            store.Rename(manager, command.Name);

            _storeRepository.Save(store);
        }
    }
}