namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public class Allocation : ValueObject
    {
        public Allocation(AllocationId allocationId, Period period, int quantity)
        {
            AllocationId = allocationId;
            Period = period;
            Quantity = quantity;
        }

        public AllocationId AllocationId { get; private set; }

        public Period Period { get; private set; }

        public int Quantity { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AllocationId;
        }
    }
}