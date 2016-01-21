namespace Phundus.Tests.Inventory.Model
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Inventory.Stores.Model;
    using Rhino.Mocks;

    public class store_concern : aggregate_concern_new<Store>
    {
        protected static inventory_factory make;

        protected static StoreId theStoreId;
        protected static Owner theOwner;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theStoreId = new StoreId();
            theOwner = make.Owner();
            sut_factory.create_using(() => new Store(theStoreId, theOwner));
        };
    }

    [Subject(typeof (Store))]
    public class when_instantiating_a_store : store_concern
    {
        private It should_have_opening_hour_nach_vereinbarung = () =>
            sut.OpeningHours.ShouldEqual("nach Vereinbarung");

        private It should_have_owner = () =>
            sut.Owner.ShouldEqual(theOwner);

        private It should_have_store_id = () =>
            sut.Id.ShouldEqual(theStoreId);
    }

    [Subject(typeof (Store))]
    public class when_changing_address : store_concern
    {
        private static string theNewAddress = "New address";

        private Because of = () => sut.ChangeAddress(theNewAddress);

        private It should_have_new_address = () =>
            sut.Address.ShouldEqual(theNewAddress);

        private It should_publish_store_address_changed = () =>
            publisher.WasToldTo(x => x.Publish(Arg<AddressChanged>.Is.NotNull));
    }
}