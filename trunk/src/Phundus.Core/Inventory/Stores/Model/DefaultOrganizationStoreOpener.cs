namespace Phundus.Inventory.Stores.Model
{
    using System;
    using Common.Domain.Model;
    using Ddd;
    using IdentityAccess.Organizations.Model;
    using Inventory.Model;
    using Repositories;

    public class DefaultOrganizationStoreOpener : ISubscribeTo<OrganizationEstablished>
    {
        private readonly IStoreRepository _storeRepository;

        public DefaultOrganizationStoreOpener(IStoreRepository storeRepository)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            _storeRepository = storeRepository;
        }

        public void Handle(OrganizationEstablished e)
        {
            var manager = new Inventory.Model.Manager(e.Initiator.InitiatorId, e.Initiator.EmailAddress,
                e.Initiator.FullName);
            var owner = new Owner(new OwnerId(e.OrganizationId), e.Name, OwnerType.Organization);
            var store = new Store(manager, new StoreId(), owner);

            _storeRepository.Add(store);

            EventPublisher.Publish(new StoreOpened(manager, store.Id, store.Owner));
        }
    }
}