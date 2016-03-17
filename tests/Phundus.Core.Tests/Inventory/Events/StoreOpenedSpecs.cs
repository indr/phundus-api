namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;

    [Subject(typeof (StoreOpened))]
    public class store_opened : inventory_domain_event_concern<StoreOpened>
    {
        private static StoreId theStoreId;
        private static Owner theOwner;

        private Establish ctx = () =>
        {
            theStoreId = new StoreId();
            theOwner = make.Owner();

            sut_factory.create_using(() =>
                new StoreOpened(theManager, theStoreId, theOwner));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_store_id = () =>
            dataMember(2).ShouldEqual(theStoreId.Id);

        private It should_have_at_3_the_owner = () =>
            dataMember(3).ShouldEqual(theOwner);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Stores.Model.StoreOpened");
    }
}