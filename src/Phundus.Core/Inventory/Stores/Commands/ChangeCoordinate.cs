namespace Phundus.Core.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAndAccess.Queries;
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
        private readonly IMemberInRole _memberInRole;

        public ChangeCoordinateHandler(IMemberInRole memberInRole, IStoreRepository storeRepository, IOwnerService ownerService)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerService, "OwnerService must be provided.");

            _memberInRole = memberInRole;
            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        public void Handle(ChangeCoordinate command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));
            var owner = _ownerService.GetByUserId(command.InitatorId);

            if (!((Equals(store.Owner, owner)) || (_memberInRole.IsActiveChief(store.Owner.OwnerId, command.InitatorId))))
                throw new AuthorizationException();

            store.ChangeCoordinate(new Coordinate(command.Latitude, command.Longitude));
        }
    }
}