namespace Phundus.Core.Inventory.Stores.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Model;
    using Repositories;

    public class ChangeOpeningHours
    {
        public int InitatorId { get; set; }
        public Guid StoreId { get; set; }
        public string OpeningHours { get; set; }
    }

    public class ChangeOpeningHoursHandler : IHandleCommand<ChangeOpeningHours>
    {
        private readonly IStoreRepository _storeRepository;

        public ChangeOpeningHoursHandler(IStoreRepository storeRepository)
        {
            AssertionConcern.AssertArgumentNotNull(storeRepository, "StoreRepository must be provided.");

            _storeRepository = storeRepository;
        }

        public void Handle(ChangeOpeningHours command)
        {
            var store = _storeRepository.GetById(new StoreId(command.StoreId));

            if (!store.Owner.OwnerId.Equals(new OwnerId(command.InitatorId)))
                throw new AuthorizationException();

            store.ChangeOpeningHours(command.OpeningHours);
        }
    }
}