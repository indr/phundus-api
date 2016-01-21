namespace Phundus.Inventory.Stores.Model
{
    using System;
    using Common.Domain.Model;
    using Ddd;
    using IdentityAccess.Organizations.Model;
    using Owners;
    using Repositories;
    using Owner = Owners.Owner;

    public class DefaultOrganizationStoreOpener : ISubscribeTo<OrganizationEstablished>
    {
        private readonly IStoreRepository _storeRepository;

        public DefaultOrganizationStoreOpener(IStoreRepository storeRepository)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            _storeRepository = storeRepository;
        }

        public void Handle(OrganizationEstablished @event)
        {
            // This should use IOwnerService.GetById(), but since we
            // use an in process and in thread event bus, it is not
            // guranteed that the IOwnerService can find the owner

            // This code will be found when the contextes are put in
            // distinct assemblies ;O)

            //var owner = _ownerService.GetById(@event.OrganizationId);
            var owner = new Owner(new OwnerId(@event.OrganizationId), @event.Name);
            var store = owner.OpenStore(new StoreId());

            _storeRepository.Add(store);

            EventPublisher.Publish(new StoreOpened());
        }
    }
}