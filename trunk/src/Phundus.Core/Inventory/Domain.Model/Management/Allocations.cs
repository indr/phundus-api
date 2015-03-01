namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class Allocations : EventSourcedEntity
    {
        private readonly IList<Allocation> _allocations = new List<Allocation>();
        private OrganizationId _organizationId;
        private ArticleId _articleId;
        private StockId _stockId;

        public void Add(Allocation allocation)
        {
            _allocations.Add(allocation);
        }


        public void When(StockCreated e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = new StockId(e.StockId);
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

        public void Remove(AllocationId allocationId)
        {
            var allocation = Get(allocationId);
            _allocations.Remove(allocation);
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic)e);
        }

        protected void When(DomainEvent e)
        {
            // Fallback
        }
    }
}