namespace Phundus.Core.Tests.Inventory
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.Inventory.Owners;
    using Core.Inventory.Services;
    using Core.Inventory.Stores.Model;
    using Core.Inventory.Stores.Repositories;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Stores.Model;
    using Rhino.Mocks;

    [Subject(typeof (DefaultOrganizationStoreOpener))]
    public class when_handled : subscriber_concern<OrganizationEstablished, DefaultOrganizationStoreOpener>
    {
        private static Guid theOrganizationGuid = Guid.NewGuid();

        private static IStoreRepository storeRepository;

        private Establish ctx =
            () =>
            {
                depends.on<IOwnerService>().WhenToldTo(x => x.GetById(theOrganizationGuid)).Return(new Owner(new OwnerId(theOrganizationGuid), "Rocks and Scissors"));
                storeRepository = depends.on<IStoreRepository>();
                @event = new OrganizationEstablished(theOrganizationGuid, "free", "Rocks and Scissors", "");
            };

        private It should_add_store_to_repository = () => storeRepository.WasToldTo(x => x.Add(Arg<Store>.Is.NotNull));

        private It should_publish_store_opened = () => publisher.WasToldTo(x => x.Publish(Arg<StoreOpened>.Is.NotNull));
    }
}