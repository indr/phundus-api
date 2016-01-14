using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Inventory.Stores.Model
{
    using Common.Domain.Model;
    using Core.Ddd;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.Inventory.Services;
    using Core.Inventory.Stores.Model;
    using Core.Inventory.Stores.Repositories;

    public class DefaultOrganizationStoreOpener : ISubscribeTo<OrganizationEstablished>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IOwnerService _ownerService;

        public DefaultOrganizationStoreOpener(IStoreRepository storeRepository, IOwnerService ownerService)
        {
            if (storeRepository == null) throw new ArgumentNullException("storeRepository");
            if (ownerService == null) throw new ArgumentNullException("ownerService");
            _storeRepository = storeRepository;
            _ownerService = ownerService;
        }

        public void Handle(OrganizationEstablished @event)
        {
            var owner = _ownerService.GetById(@event.OrganizationId);
            var store = owner.OpenStore(new StoreId());

            _storeRepository.Add(store);

            EventPublisher.Publish(new StoreOpened());
        }
    }
}
