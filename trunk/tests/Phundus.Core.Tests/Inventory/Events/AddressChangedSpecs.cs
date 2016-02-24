namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;

    [Subject(typeof (AddressChanged))]
    public class address_changed : inventory_domain_event_concern<AddressChanged>
    {
        private static Manager theManager;
        private static StoreId theStoreId;
        private static string theAddress;

        private Establish ctx = () =>
        {
            theManager = make.Manager();
            theStoreId = new StoreId();
            theAddress = "The address";

            sut_factory.create_using(() =>
                new AddressChanged(theManager, theStoreId, theAddress));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager);

        private It should_have_at_2_the_store_id = () =>
            dataMember(2).ShouldEqual(theStoreId.Id);

        private It should_have_at_3_the_address = () =>
            dataMember(3).ShouldEqual(theAddress);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Stores.Model.AddressChanged");
    }
}