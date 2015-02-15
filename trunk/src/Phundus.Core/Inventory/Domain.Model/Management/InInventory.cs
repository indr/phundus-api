namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class InInventory
    {
        private readonly ArticleId _articleId;
        private readonly OrganizationId _organizationId;
        private readonly StockId _stockId;
        
        private readonly QuantityPeriods _quantites = new QuantityPeriods();

        public InInventory(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
        }

        public ICollection<QuantityAsOf> Quantities { get { return _quantites.GetQuantityAsOf(); } }

        public IDomainEvent Change(Period period, int change, string comment)
        {
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than 0.");

            var asOfUtc = period.FromUtc;
            var newTotal = _quantites.QuantityAsOf(asOfUtc) + change;

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
            _quantites.Add(new QuantityPeriod(new Period(e.AsOfUtc), e.Change));
        }

        public void When(QuantityInInventoryDecreased e)
        {
            _quantites.Add(new QuantityPeriod(new Period(e.AsOfUtc), e.Change * -1));
        }
    }
}