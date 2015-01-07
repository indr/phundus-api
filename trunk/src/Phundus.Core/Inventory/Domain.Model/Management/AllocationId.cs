namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using Common.Domain.Model;

    public class AllocationId : Identity<Guid>
    {
        public AllocationId() : base(Guid.NewGuid())
        {
        }

        public AllocationId(Guid id) : base(id)
        {
        }
    }
}