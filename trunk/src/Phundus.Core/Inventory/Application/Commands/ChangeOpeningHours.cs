namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Collaborators;
    using Model.Stores;

    public class ChangeOpeningHours : ICommand
    {
        public ChangeOpeningHours(InitiatorId initiatorId, StoreId storeId, string openingHours)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (openingHours == null) throw new ArgumentNullException("openingHours");

            InitiatorId = initiatorId;
            StoreId = storeId;
            OpeningHours = openingHours;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public StoreId StoreId { get; protected set; }
        public string OpeningHours { get; protected set; }
    }

    public class ChangeOpeningHoursHandler : IHandleCommand<ChangeOpeningHours>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IStoreRepository _storeRepository;

        public ChangeOpeningHoursHandler(IStoreRepository storeRepository, ICollaboratorService collaboratorService)
        {
            _storeRepository = storeRepository;
            _collaboratorService = collaboratorService;
        }

        [Transaction]
        public void Handle(ChangeOpeningHours command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _collaboratorService.Manager(command.InitiatorId, store.OwnerId);

            store.ChangeOpeningHours(manager, command.OpeningHours);

            _storeRepository.Save(store);
        }
    }
}