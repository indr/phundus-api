namespace Phundus.Tests.Inventory.Model
{
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;
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
            storeRepository = depends.on<IStoreRepository>();
            @event = new OrganizationEstablished(
                new Founder(new UserId(), "founder@test.phundus.ch", "The Founder"), theOrganizationGuid,
                "Rocks and Scissors", OrganizationPlan.Free, true);
        };

        private It should_add_store_to_repository = () =>
            storeRepository.WasToldTo(x => x.Add(Arg<Store>.Is.NotNull));
    }
}