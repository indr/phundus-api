namespace Phundus.Inventory.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Stores;
    using Stores.Model;

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
        private readonly IStoreRepository _storeRepository;
        private readonly IUserInRole _userInRole;

        public ChangeCoordinateHandler(IStoreRepository storeRepository, IUserInRole userInRole)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            _storeRepository = storeRepository;
            _userInRole = userInRole;
        }

        [Transaction]
        public void Handle(ChangeCoordinate command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _userInRole.Manager(command.InitiatorId, store.OwnerId);

            store.ChangeCoordinate(manager, new Coordinate(command.Latitude, command.Longitude));

            _storeRepository.Save(store);
        }
    }
}