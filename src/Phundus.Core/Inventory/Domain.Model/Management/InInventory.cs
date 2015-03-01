namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using AutoMapper;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Itenso.TimePeriod;
    using NHibernate.Linq;

    public class InInventory : EventSourcedEntity
    {
        //private readonly QuantityPeriods _qps = new QuantityPeriods();
        private ArticleId _articleId;
        private OrganizationId _organizationId;
        private StockId _stockId;

        private IList<TimeRange> _ranges = new List<TimeRange>(); 
        

        public ICollection<QuantityAsOf> QuantityAsOf
        {
            get
            {
                var result = new QuantitiesAsOf();

                foreach (var range in _ranges)
                {
                    result.Add(range);
                }

                return result.Quantities;
            }
        }

        public ICollection<ITimePeriod> TimePeriods
        {
            get { return new List<ITimePeriod>(_ranges); }
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

            var newTotal = _ranges.Count(p => p.HasInside(asOfUtc)) + change;

            Apply(CreateQuantityInInventoryChangedEvent(change, comment, newTotal, asOfUtc));            
        }

        private IDomainEvent CreateQuantityInInventoryChangedEvent(int change, string comment, int newTotal, DateTime asOfUtc)
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
                _ranges.Add(new TimeRange(e.AsOfUtc, DateTime.MaxValue));
        }

        public void When(QuantityInInventoryDecreased e)
        {
            var substractor = new TimePeriodSubtractor<TimeRange>();
            substractor.SubtractPeriods()


            for (int idx = 0; idx < e.Change; idx++)
            foreach (var each in _ranges)
            {
                if ((each.Start <= e.AsOfUtc) && (each.End == DateTime.MaxValue))
                {
                    each.ShrinkEndTo(e.AsOfUtc);
                    break;
                }
            }
        }

        /// <summary>
        /// TODO: Move to domain service?
        /// </summary>
        /// <param name="allocations"></param>
        /// <returns></returns>
        public QuantityPeriods ComputeAvailabilities(Allocations allocations)
        {
            return null;
            //var result = new QuantityPeriods(_qps);

            // TODO: How is responsible for which allocation status are taken into account?
            // TODO: Implement IEnumerable<Allocation>
            //foreach (var each in allocations.Items.Where(p => p.Status != AllocationStatus.Allocated))
            //{
            //    result.Sub(each.Period, each.Quantity);
            //}
            //return result;
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