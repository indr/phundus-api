namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Itenso.TimePeriod;

    public class QuantityInInventory : EventSourcedEntity
    {
        private ArticleId _articleId;
        private OrganizationId _organizationId;
        private IList<TimeRange> _periods = new List<TimeRange>();
        private StockId _stockId;

        public ICollection<QuantityAsOf> QuantityAsOf
        {
            get
            {
                var result = new QuantitiesAsOf();

                foreach (var range in _periods)
                {
                    result.Add(range);
                }

                return result.Quantities;
            }
        }

        public ICollection<ITimePeriod> Periods
        {
            get { return new List<ITimePeriod>(_periods); }
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        protected void When(DomainEvent e)
        {
            // Fallback
        }

        public void When(StockCreated e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = new StockId(e.StockId);
        }

        public void Change(Period period, int change, string comment)
        {
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than 0.");

            var asOfUtc = period.FromUtc;

            var newTotal = _periods.Count(p => p.HasInside(asOfUtc)) + change;

            Apply(CreateQuantityInInventoryChangedEvent(change, comment, newTotal, asOfUtc));
        }

        private IDomainEvent CreateQuantityInInventoryChangedEvent(int change, string comment, int newTotal,
            DateTime asOfUtc)
        {
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
            for (int idx = 0; idx < e.Change; idx++)
                _periods.Add(new TimeRange(e.AsOfUtc, DateTime.MaxValue));
        }

        public void When(QuantityInInventoryDecreased e)
        {
            for (int idx = 0; idx < e.Change; idx++)
                foreach (var each in _periods)
                {
                    if ((each.Start <= e.AsOfUtc) && (each.End == DateTime.MaxValue))
                    {
                        each.ShrinkEndTo(e.AsOfUtc);
                        break;
                    }
                }
        }
    }
}