namespace Phundus.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;
    using Services;

    public class ChangeAddress
    {
        public UserGuid InitatorId { get; set; }
        public Guid StoreId { get; set; }
        public string Address { get; set; }
    }

    public class ChangeAddressHandler : IHandleCommand<ChangeAddress>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IOwnerService _ownerService;
        private readonly IMemberInRole _memberInRole;

        public ChangeAddressHandler(IMemberInRole memberInRole, IStoreRepository storeRepository, IOwnerService ownerService)
        {
            AssertionConcern.AssertArgumentNotNull(memberInRole, "MemberInRole must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(ownerService, "OwnerService must be provided.");

            _memberInRole = memberInRole;
            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        public void Handle(ChangeAddress command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));
            var owner = _ownerService.GetByUserId(command.InitatorId);

            if (!((Equals(store.Owner, owner)) || (_memberInRole.IsActiveChief(store.Owner.OwnerId, command.InitatorId))))
                throw new AuthorizationException();

            store.ChangeAddress(command.Address);
        }
    }
}