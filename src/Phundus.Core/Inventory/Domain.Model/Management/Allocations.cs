namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Allocations
    {
        private readonly IList<Allocation> _allocations = new List<Allocation>(); 

        public void Add(Allocation allocation)
        {
            _allocations.Add(allocation);
        }

        public Allocation Get(AllocationId allocationId)
        {
            var result = _allocations.SingleOrDefault(p => Equals(p.AllocationId, allocationId));
            if (result == null)
                throw new AllocationNotFoundException(allocationId);
            return result;
        }

        public ICollection<Allocation> Items
        {
            get { return new ReadOnlyCollection<Allocation>(_allocations); }
        }
    }
}