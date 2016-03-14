namespace Phundus.Inventory.Projections
{
    using System;
    using Common.Eventing;
    using Common.Projecting;
    using Stores.Model;

    public class StoreListProjection : ProjectionBase<StoreListData>,
        ISubscribeTo<StoreOpened>,
        ISubscribeTo<StoreRenamed>
    {
        public void Handle(StoreOpened e)
        {
            Insert(x =>
            {
                x.StoreId = e.StoreId;
                x.OwnerId = e.Owner.OwnerId.Id;
                x.OwnerType = e.Owner.Type.ToString().ToLowerInvariant();
            });
        }

        public void Handle(StoreRenamed e)
        {
            Update(e.StoreId, x =>
                x.Name = e.Name);
        }
    }

    public class StoreListData
    {
        public virtual Guid StoreId { get; set; }
        public virtual Guid OwnerId { get; set; }
        public virtual string OwnerType { get; set; }
        public virtual string Name { get; set; }
    }
}