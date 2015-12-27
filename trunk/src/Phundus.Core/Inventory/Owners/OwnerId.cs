namespace Phundus.Core.Inventory.Owners
{
    using System;
    using Common.Domain.Model;

    public class OwnerId : Identity<Guid>
    {
        public OwnerId(Guid value) : base(value)
        {
        }

        protected OwnerId()
        {
        }
    }
}