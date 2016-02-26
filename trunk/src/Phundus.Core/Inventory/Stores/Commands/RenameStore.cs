namespace Phundus.Inventory.Stores.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;
    using Services;

    public class RenameStore
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
        private readonly IStoreRepository _storeRepository;
        private readonly IUserInRole _userInRole;

        public RenameStoreHandler(IStoreRepository storeRepository, IUserInRole userInRole)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (userInRole == null) throw new ArgumentNullException("userInRole");

            _storeRepository = storeRepository;
            _userInRole = userInRole;
        }

        public void Handle(RenameStore command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _userInRole.Manager(command.InitiatorId, store.Owner.OwnerId);

            store.Rename(manager, command.Name);

            _storeRepository.Save(store);
        }
    }
}