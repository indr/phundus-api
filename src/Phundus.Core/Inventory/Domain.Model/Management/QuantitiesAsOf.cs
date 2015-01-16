namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common;
    using Common.Domain.Model;

    public class QuantitiesAsOf
    {
        private List<QuantityAsOf> _quantities = new List<QuantityAsOf>();

        public ICollection<QuantityAsOf> Quantities
        {
            get { return new ReadOnlyCollection<QuantityAsOf>(_quantities); }
        }

        public int GetTotalAsOf(DateTime asOfUtc)
        {
            AssertionConcern.AssertArgumentNotNull(asOfUtc, "As of utc must be provided.");

            var atAsOfOrLatest = FindAtAsOfOrLatestBefore(asOfUtc);

            if (atAsOfOrLatest == null)
                return 0;

            return atAsOfOrLatest.Total;
        }

        public bool HasQuantityInPeriod(Period period, int quantity)
        {
            var atStart = FindAtAsOfOrLatestBefore(period.FromUtc);
            if (atStart.Total < quantity)
                return false;

            return _quantities.Where(p => (p.AsOfUtc > period.FromUtc) && (p.AsOfUtc <= period.ToUtc))
                .All(each => each.Total >= quantity);
        }

        public void ChangeAsOf(int change, DateTime asOfUtc)
        {
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater than zero.");
            AssertionConcern.AssertArgumentNotEmpty(asOfUtc, "As of utc must be provided.");

            var atAsOfOrLatest = FindAtAsOfOrLatestBefore(asOfUtc);

            QuantityAsOf quantityAsOf = null;
            if (atAsOfOrLatest == null)
            {
                quantityAsOf = new QuantityAsOf(change, change, asOfUtc);
                _quantities.Add(quantityAsOf);
            }
            else if (atAsOfOrLatest.AsOfUtc == asOfUtc)
            {
                quantityAsOf = atAsOfOrLatest;
                quantityAsOf.AddChange(change);
            }
            else
            {
                quantityAsOf = new QuantityAsOf(change, atAsOfOrLatest.Total + change, asOfUtc);
                _quantities.Add(quantityAsOf);
            }

            UpdateFutures(change, asOfUtc);

            _quantities = _quantities.OrderBy(ks => ks.AsOfUtc).ToList();
        }

        private QuantityAsOf FindAtAsOfOrLatestBefore(DateTime asOfUtc)
        {
            return _quantities.Where(p => p.AsOfUtc <= asOfUtc).OrderByDescending(ks => ks.AsOfUtc).FirstOrDefault();
        }

        private void UpdateFutures(int change, DateTime asOfUtc)
        {
            foreach (var each in _quantities.Where(p => p.AsOfUtc > asOfUtc))
            {
                each.AddToTotal(change);
            }
        }
    }
}