namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Collaborators;
    using Model.Stores;

    public class ChangeCoordinate : ICommand
    {
        public ChangeCoordinate(InitiatorId initiatorId, StoreId storeId, decimal latitude, decimal longitude)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (storeId == null) throw new ArgumentNullException("storeId");
            InitiatorId = initiatorId;
            StoreId = storeId;
            Latitude = latitude;
            Longitude = longitude;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public StoreId StoreId { get; protected set; }
        public decimal Latitude { get; protected set; }
        public decimal Longitude { get; protected set; }
    }

    public class ChangeCoordinateHandler : IHandleCommand<ChangeCoordinate>
    {
        private readonly ICollaboratorService _collaboratorService;
        private readonly IStoreRepository _storeRepository;

        public ChangeCoordinateHandler(IStoreRepository storeRepository, ICollaboratorService collaboratorService)
        {
            _storeRepository = storeRepository;
            _collaboratorService = collaboratorService;
        }

        [Transaction]
        public void Handle(ChangeCoordinate command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _collaboratorService.Manager(command.InitiatorId, store.OwnerId);

            store.ChangeCoordinate(manager, new Coordinate(command.Latitude, command.Longitude));

            _storeRepository.Save(store);
        }
    }
}