namespace Phundus.Core.Inventory.Stores.Model
{
    using System;
    using Common.Domain.Model;

    public class StoreId : Identity<Guid>
    {
        public StoreId() : base(Guid.NewGuid())
        {
        }

        public StoreId(Guid value) : base(value)
        {
        }
    }
}