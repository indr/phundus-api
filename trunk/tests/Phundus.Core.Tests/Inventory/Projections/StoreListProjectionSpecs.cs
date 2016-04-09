namespace Phundus.Tests.Inventory.Projections
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Articles;
    using Phundus.Inventory.Projections;
    using Phundus.Inventory.Stores.Model;

    public class store_list_projection_concern : inventory_projection_concern<StoreListProjection, StoreListData>
    {
    }

    [Subject(typeof (StoreListProjection))]
    public class slp_when_handling_store_opened : store_list_projection_concern
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

    [Subject(typeof (StoreListProjection))]
    public class slp_when_handling_store_renamed : store_list_projection_concern
    {
        private Because of = () =>
            sut.Handle(new StoreRenamed(theManager, theStoreId, "The new name"));

        private It should_update_entity = () =>
            updated(theStoreId.Id, x =>
                x.Name.ShouldEqual("The new name"));
    }
}