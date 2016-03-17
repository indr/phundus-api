namespace Phundus.Tests.Inventory.Projections
{
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Model.Stores;
    using Phundus.Inventory.Projections;
    using Phundus.Inventory.Stores.Model;

    public class store_details_projection_concern :
        inventory_projection_concern<StoreDetailsProjection, StoreDetailsData>
    {
    }

    [Subject(typeof (StoreDetailsProjection))]
    public class when_handling_contact_details_changed : store_details_projection_concern
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

    [Subject(typeof (StoreDetailsProjection))]
    public class when_handling_coordinate_changed : store_details_projection_concern
    {
        private Because of = () =>
            sut.Handle(new CoordinateChanged(theManager, theStoreId, new Coordinate(1.11m, 2.22m)));

        private It should_update_entity = () =>
            updated(theStoreId.Id, x =>
            {
                x.Latitude.ShouldEqual(1.11m);
                x.Longitude.ShouldEqual(2.22m);
            });
    }

    [Subject(typeof (StoreDetailsProjection))]
    public class when_handling_opening_hours_changed : store_details_projection_concern
    {
        private Because of = () =>
            sut.Handle(new OpeningHoursChanged(theManager, theStoreId, "The opening hours"));

        private It should_update_entity = () =>
            updated(theStoreId.Id, x =>
                x.OpeningHours.ShouldEqual("The opening hours"));
    }

    [Subject(typeof (StoreDetailsProjection))]
    public class when_handling_store_opened : store_details_projection_concern
    {
        private Because of = () =>
            sut.Handle(new StoreOpened(theManager, theStoreId, theOwner));

        private It should_insert_entity = () =>
            inserted(x =>
            {
                x.StoreId.ShouldEqual(theStoreId.Id);
                x.OwnerId.ShouldEqual(theOwnerId.Id);
                x.OwnerType.ShouldEqual(theOwner.Type.ToString().ToLowerInvariant());
            });
    }

    [Subject(typeof (StoreRenamed))]
    public class when_handling_store_renamed : store_details_projection_concern
    {
        private Because of = () =>
            sut.Handle(new StoreRenamed(theManager, theStoreId, "The new name"));

        private It should_update_entity = () =>
            updated(theStoreId.Id, x =>
                x.Name.ShouldEqual("The new name"));
    }
}