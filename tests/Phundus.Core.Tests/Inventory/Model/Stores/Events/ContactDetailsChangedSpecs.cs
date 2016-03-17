namespace Phundus.Tests.Inventory.Model.Stores.Events
{
    using Common.Domain.Model;
    using Inventory.Events;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Stores;

    [Subject(typeof (ContactDetailsChanged))]
    public class ContactDetailsChangedSpecs : inventory_domain_event_concern<ContactDetailsChanged>
    {
        private static StoreId theStoreId;
        private static OwnerId theOwnerId;

        private Establish ctx = () =>
        {
            theStoreId = new StoreId();
            theOwnerId = new OwnerId();

            sut_factory.create_using(() =>
                new ContactDetailsChanged(theManager, theOwnerId, theStoreId,
                    new ContactDetails("emailAddress", "phoneNumber", new PostalAddress(
                        "line1", "line2", "street", "postcode", "city"))));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_10_the_city = () =>
            dataMember(10).ShouldEqual("city");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_owner_id = () =>
            dataMember(2).ShouldEqual(theOwnerId.Id);

        private It should_have_at_3_the_store_id = () =>
            dataMember(3).ShouldEqual(theStoreId.Id);

        private It should_have_at_4_the_email_address = () =>
            dataMember(4).ShouldEqual("emailAddress");

        private It should_have_at_5_the_phone_number = () =>
            dataMember(5).ShouldEqual("phoneNumber");

        private It should_have_at_6_the_line_1 = () =>
            dataMember(6).ShouldEqual("line1");

        private It should_have_at_7_the_line_2 = () =>
            dataMember(7).ShouldEqual("line2");

        private It should_have_at_8_the_street = () =>
            dataMember(8).ShouldEqual("street");

        private It should_have_at_9_the_postcode = () =>
            dataMember(9).ShouldEqual("postcode");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Model.Stores.ContactDetailsChanged");
    }
}