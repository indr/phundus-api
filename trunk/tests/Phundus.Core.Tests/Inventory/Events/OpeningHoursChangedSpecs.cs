namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;

    [Subject(typeof (OpeningHoursChanged))]
    public class opening_hours_changed : inventory_domain_event_concern<OpeningHoursChanged>
    {
        private static Manager theManager;
        private static string theOpeningHours;
        private static StoreId theStoreId;

        private Establish ctx = () =>
        {
            theManager = make.Manager();
            theStoreId = new StoreId();
            theOpeningHours = "The opening hours";
            sut_factory.create_using(() =>
                new OpeningHoursChanged(theManager, theStoreId, theOpeningHours));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager);

        private It should_have_at_2_the_store_id = () =>
            dataMember(2).ShouldEqual(theStoreId.Id);

        private It should_have_at_3_the_opening_hours = () =>
            dataMember(3).ShouldEqual(theOpeningHours);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Stores.Model.OpeningHoursChanged");
    }
}