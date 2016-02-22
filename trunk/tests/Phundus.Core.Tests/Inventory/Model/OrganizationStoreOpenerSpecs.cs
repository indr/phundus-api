namespace Phundus.Tests.Inventory
{
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.Inventory.Services;
    using Phundus.Inventory.Stores.Model;
    using Phundus.Inventory.Stores.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (DefaultOrganizationStoreOpener))]
    public class when_handled : subscriber_concern<OrganizationEstablished, DefaultOrganizationStoreOpener>
    {
        private static OrganizationId theOrganizationGuid = new OrganizationId();

        private static IStoreRepository storeRepository;

        private Establish ctx = () =>
        {
            depends.on<IOwnerService>()
                .WhenToldTo(x => x.GetById(theOrganizationGuid.Id))
                .Return(new Owner(new OwnerId(theOrganizationGuid.Id), "Rocks and Scissors", OwnerType.Organization));
            storeRepository = depends.on<IStoreRepository>();
            @event = new OrganizationEstablished(
                new Founder(new UserId(), "founder@test.phundus.ch", "The Founder"), theOrganizationGuid,
                "Rocks and Scissors", OrganizationPlan.Free);
        };

        private It should_add_store_to_repository = () => storeRepository.WasToldTo(x => x.Add(Arg<Store>.Is.NotNull));

        private It should_publish_store_opened = () =>
            Published<StoreOpened>(p => p != null);
    }
}