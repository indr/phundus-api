namespace Phundus.Tests.Inventory.Projections
{
    using Common.Domain.Model;
    using Common.Projecting;
    using IdentityAccess;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Stores;
    using Phundus.Inventory.Projections;

    public class inventory_projection_concern<TClass, TData> : projection_concern<TClass, TData>
        where TClass : ProjectionBase where TData : new()
    {
        protected static identityaccess_factory make;
        protected static Manager theManager;
        protected static OwnerId theOwnerId;
        protected static StoreId theStoreId;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);
            theManager = new Manager(new UserId(), "manager@test.phundus.ch", "The Manager");
            theOwnerId = new OwnerId();
            theStoreId = new StoreId();
        };
    }

    public class store_details_projection_concern<TData> : inventory_projection_concern<StoreDetailsProjection, TData>
        where TData : new()
    {
    }

    [Subject(typeof (StoreDetailsProjection))]
    public class when_handling_contact_details_changed : store_details_projection_concern<StoreDetailsData>
    {
        private Because of = () =>
            sut.Handle(new ContactDetailsChanged(theManager, theOwnerId, theStoreId,
                new ContactDetails("emailAddress", "phoneNumber",
                    new PostalAddress("line1", "line2", "street", "postcode", "city"))));

        private It should_update_entity = () =>
            updated(theStoreId.Id, x =>
            {
                x.EmailAddress.ShouldEqual("emailAddress");
                x.PhoneNumber.ShouldEqual("phoneNumber");
                x.Line1.ShouldEqual("line1");
                x.Line2.ShouldEqual("line2");
                x.Street.ShouldEqual("street");
                x.Postcode.ShouldEqual("postcode");
                x.City.ShouldEqual("city");
                x.PostalAddress.ShouldNotBeEmpty();
            });
    }
}