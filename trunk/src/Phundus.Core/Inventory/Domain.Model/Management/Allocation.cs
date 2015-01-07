namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Allocation : ValueObject
    {
        public Allocation(AllocationId allocationId)
        {
            AllocationId = allocationId;
        }

        public AllocationId AllocationId { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AllocationId;
        }
    }
}