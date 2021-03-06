﻿namespace Phundus.Tests.Inventory.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;

    [Subject(typeof (StoreRenamed))]
    public class store_renamed : inventory_domain_event_concern<StoreRenamed>
    {
        private static StoreId theStoreId;
        private static string theName;

        private Establish ctx = () =>
        {
            theStoreId = new StoreId();
            theName = "The name";
            sut_factory.create_using(() =>
                new StoreRenamed(theManager, theStoreId, theName));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_store_id = () =>
            dataMember(2).ShouldEqual(theStoreId.Id);

        private It should_have_at_3_the_name = () =>
            dataMember(3).ShouldEqual(theName);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Inventory.Stores.Model.StoreRenamed");
    }
}