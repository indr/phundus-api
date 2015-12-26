namespace Phundus.Core.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;

    public class ChangeCoordinate
    {
        public int InitatorId { get; set; }
        public Guid StoreId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class ChangeCoordinateHandler : IHandleCommand<ChangeCoordinate>
    {
        private readonly IStoreRepository _storeRepository;

        public ChangeCoordinateHandler(IStoreRepository storeRepository)
        {
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");
           
            _storeRepository = storeRepository;
        }

        public void Handle(ChangeCoordinate command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));

            if (!store.Owner.OwnerId.Equals(new OwnerId(command.InitatorId)))
                throw new AuthorizationException();

            store.ChangeCoordinate(new Coordinate(command.Latitude, command.Longitude));
        }
    }
}