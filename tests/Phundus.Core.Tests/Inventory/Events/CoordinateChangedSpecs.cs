namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;

    [Subject(typeof (CoordinateChanged))]
    public class coordinate_changed : inventory_domain_event_concern<CoordinateChanged>
    {
        private static Manager theManager;
        private static StoreId theStoreId;
        private static Coordinate theCoordinate;

        private Establish ctx = () =>
        {
            theManager = make.Manager();
            theStoreId = new StoreId();
            theCoordinate = new Coordinate(1, 2);
            sut_factory.create_using(() =>
                new CoordinateChanged(theManager, theStoreId, theCoordinate));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager);

        private It should_have_at_2_the_store_id = () =>
            dataMember(2).ShouldEqual(theStoreId.Id);

        private It should_have_at_3_the_latitude = () =>
            dataMember(3).ShouldEqual(theCoordinate.Latitude);

        private It should_have_at_4_the_longitude = () =>
            dataMember(4).ShouldEqual(theCoordinate.Longitude);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Stores.Model.CoordinateChanged");
    }
}