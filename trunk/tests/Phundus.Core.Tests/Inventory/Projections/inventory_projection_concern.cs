namespace Phundus.Tests.Inventory.Projections
{
    using Common.Domain.Model;
    using Common.Projecting;
    using Machine.Specifications;
    using Phundus.Inventory.Model;

    public class inventory_projection_concern<TProjection, TData> : projection_concern<TProjection, TData>
        where TProjection : ProjectionBase where TData : new()
    {
        protected static inventory_factory make;
        protected static Manager theManager;
        protected static Owner theOwner;
        protected static OwnerId theOwnerId;
        protected static StoreId theStoreId;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);
            theManager = make.Manager();
            theOwner = make.Owner();
            theOwnerId = theOwner.OwnerId;
            theStoreId = new StoreId();
        };
    }
}