namespace Phundus.Core.Inventory.Stores.Model
{
    using System;
    using Common;
    using Common.Domain.Model;

    public class OwnerId : Identity<Guid>
    {
        public OwnerId(Guid value) : base(value)
        {
        }

        public OwnerId(int value) : base(value.ToGuid())
        {
        }

        protected OwnerId()
        {
        }
    }
}