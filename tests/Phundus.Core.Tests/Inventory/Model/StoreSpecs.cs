namespace Phundus.Tests.Inventory.Model
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Stores;
    using Phundus.Inventory.Stores.Model;

    public class store_concern : aggregate_root_concern<Store>
    {
        protected static inventory_factory make;

        protected static StoreId theStoreId;
        protected static Owner theOwner;
        protected static Manager theManager;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theStoreId = new StoreId();
            theOwner = make.Owner();
            theManager = make.Manager();
            sut_factory.create_using(() => new Store(theManager, theStoreId, theOwner));
        };
    }

    [Subject(typeof (Store))]
    public class when_opening_a_store : store_concern
    {
        private It should_have_empty_contact_details = () =>
        {
            sut.ContactDetails.EmailAddress.ShouldBeNull();
            sut.ContactDetails.PhoneNumber.ShouldBeNull();
            sut.ContactDetails.PostalAddress.ShouldBeNull();
        };

        private It should_have_mutating_event_store_opened = () =>
            mutatingEvent<StoreOpened>(p =>
                Equals(p.Manager, theManager)
                && Equals(p.Owner, theOwner)
                && p.StoreId == theStoreId.Id);

        private It should_have_owner_id = () =>
            sut.OwnerId.ShouldEqual(theOwner.OwnerId);

        private It should_have_store_id = () =>
            sut.StoreId.ShouldEqual(theStoreId);
    }

    [Subject(typeof (Store))]
    public class when_changing_address : store_concern
    {
        private static string theNewAddress = "New address";

        private Because of = () =>
            sut.ChangeAddress(theManager, theNewAddress);

        private It should_have_mutating_event_address_changed = () =>
            mutatingEvent<AddressChanged>(p =>
                Equals(p.Manager.UserId, theManager.UserId)
                && p.StoreId == theStoreId.Id
                && p.Address == theNewAddress);

        private It should_have_the_new_address = () =>
            sut.Address.ShouldEqual(theNewAddress);
    }

    [Subject(typeof (Store))]
    public class when_changing_contact_details : store_concern
    {
        private static ContactDetails theContactDetails = new ContactDetails("emailAddress", "phoneNumber",
            new PostalAddress("line1", "line2", "street", "postcode", "city"));

        private Because of = () =>
            sut.ChangeContactDetails(theManager, theContactDetails);

        private It should_have_mutating_event_contact_details_changed = () =>
            mutatingEvent<ContactDetailsChanged>(e =>
            {
                e.Manager.ShouldEqual(theManager);
                e.OwnerId.ShouldEqual(theOwner.OwnerId.Id);
                e.StoreId.ShouldEqual(theStoreId.Id);
                e.EmailAddress.ShouldEqual("emailAddress");
                e.PhoneNumber.ShouldEqual("phoneNumber");
                e.Line1.ShouldEqual("line1");
                e.Line2.ShouldEqual("line2");
                e.Street.ShouldEqual("street");
                e.Postcode.ShouldEqual("postcode");
                e.City.ShouldEqual("city");
            });

        private It should_have_the_new_contact_details = () =>
            sut.ContactDetails.ShouldEqual(theContactDetails);
    }

    [Subject(typeof (Store))]
    public class when_renaming_a_store : store_concern
    {
        private static string theNewName = "The new name";

        private Because of = () =>
            sut.Rename(theManager, theNewName);

        private It should_have_mutating_event_store_renamed = () =>
            mutatingEvent<StoreRenamed>(p =>
                Equals(p.Manager.UserId, theManager.UserId)
                && p.StoreId == theStoreId.Id
                && p.Name == theNewName);

        private It should_have_the_new_name = () =>
            sut.Name.ShouldEqual(theNewName);
    }
}