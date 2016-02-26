namespace Phundus.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;
    using Services;

    public class OpenStore
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

        public InitiatorId InitiatorId { get; private set; }
        public OwnerId OwnerId { get; private set; }
        public StoreId StoreId { get; private set; }
    }

    public class OpenStoreHandler : IHandleCommand<OpenStore>
    {
        private readonly IOwnerService _ownerService;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserInRole _userInRole;

        public OpenStoreHandler(IStoreRepository storeRepository, IUserInRole userInRole, IOwnerService ownerService)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (ownerService == null) throw new ArgumentNullException("ownerService");

            _storeRepository = storeRepository;
            _userInRole = userInRole;
            _ownerService = ownerService;
        }

        public void Handle(OpenStore command)
        {
            var manager = _userInRole.Manager(command.InitiatorId, command.OwnerId);
            var owner = _ownerService.GetById(command.OwnerId);

            var store = new Store(manager, command.StoreId, owner);

            _storeRepository.Add(store);
        }
    }
}