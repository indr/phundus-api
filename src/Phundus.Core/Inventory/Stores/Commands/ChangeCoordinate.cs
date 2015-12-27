namespace Phundus.Core.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Model;
    using Repositories;
    using Services;

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
        private readonly IOwnerService _ownerService;

        public ChangeCoordinateHandler(IStoreRepository storeRepository, IOwnerService ownerService)
        {
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerService, "OwnerService must be provided.");

            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        public void Handle(ChangeCoordinate command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));
            var owner = _ownerService.GetByUserId(command.InitatorId);

            if (!Equals(store.Owner, owner))
                throw new AuthorizationException();

            store.ChangeCoordinate(new Coordinate(command.Latitude, command.Longitude));
        }
    }
}