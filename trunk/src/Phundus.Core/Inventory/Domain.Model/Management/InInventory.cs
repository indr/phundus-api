namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class InInventory
    {
        private readonly ArticleId _articleId;
        private readonly OrganizationId _organizationId;
        private readonly StockId _stockId;
        
        private readonly QuantityPeriods _quantityPeriods = new QuantityPeriods();

        public InInventory(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
        }

        public ICollection<QuantityAsOf> QuantityAsOf { get { return _quantityPeriods.GetQuantityAsOf(); } }

        public QuantityPeriods QuantityPeriods { get { return _quantityPeriods; } } 

        public IDomainEvent Change(Period period, int change, string comment)
        {
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than 0.");

            var asOfUtc = period.FromUtc;
            var newTotal = _quantityPeriods.QuantityAsOf(asOfUtc) + change;

            IDomainEvent e;

            if (change > 0)
                e = new QuantityInInventoryIncreased(_organizationId, _articleId, _stockId, change,
                    newTotal, asOfUtc, comment);
            else
                e = new QuantityInInventoryDecreased(_organizationId, _articleId, _stockId, change*-1,
                    newTotal, asOfUtc, comment);

            return e;
        }

        public void When(QuantityInInventoryIncreased e)
        {
            _quantityPeriods.Add(new QuantityPeriod(new Period(e.AsOfUtc), e.Change));
        }

        public void When(QuantityInInventoryDecreased e)
        {
            _quantityPeriods.Add(new QuantityPeriod(new Period(e.AsOfUtc), e.Change * -1));
        }

        /// <summary>
        /// TODO: Move to domain service?
        /// </summary>
        /// <param name="allocations"></param>
        /// <returns></returns>
        public QuantityPeriods ComputeAvailabilities(Allocations allocations)
        {
            var result = new QuantityPeriods(_quantityPeriods);

            // TODO: How is responsible for which allocation status are taken into account?
            // TODO: Implement IEnumerable<Allocation>
            foreach (var each in allocations.Items.Where(p => p.Status != AllocationStatus.Allocated))
            {
                result.Sub(new QuantityPeriod(each.Period, each.Quantity));
            }
            return result;
        }
    }
}