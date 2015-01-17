namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;

    public class AllocationNotFoundException : Exception
    {
        public AllocationNotFoundException(AllocationId allocationId)
            : base(String.Format("Allocation {0} not found.", allocationId.Id))
        {
        }
    }
}