namespace Phundus.Inventory.Stores.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Repositories;
    using Services;

    public class ChangeOpeningHours
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
        private readonly IStoreRepository _storeRepository;
        private readonly IUserInRole _userInRole;

        public ChangeOpeningHoursHandler(IStoreRepository storeRepository, IUserInRole userInRole)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (userInRole == null) throw new ArgumentNullException("userInRole");

            _storeRepository = storeRepository;
            _userInRole = userInRole;
        }

        public void Handle(ChangeOpeningHours command)
        {
            var store = _storeRepository.GetById(command.StoreId);
            var manager = _userInRole.Manager(command.InitiatorId, store.Owner.OwnerId);

            store.ChangeOpeningHours(command.OpeningHours);
        }
    }
}