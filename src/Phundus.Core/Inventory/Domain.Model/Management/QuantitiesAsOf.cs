namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;

    public class QuantitiesAsOf
    {
        private List<QuantityAsOf> _quantities = new List<QuantityAsOf>();

        private QuantityAsOf FindAtAsOfOrLatestBefore(DateTime asOfUtc)
        {
            return _quantities.Where(p => p.AsOfUtc <= asOfUtc).OrderByDescending(ks => ks.AsOfUtc).FirstOrDefault();
        }

        public void ChangeAsOf(int change, DateTime asOfUtc)
        {
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater than zero.");
            AssertionConcern.AssertArgumentNotEmpty(asOfUtc, "As of utc must be provided.");

            var atAsOfOrLatest = FindAtAsOfOrLatestBefore(asOfUtc);
            if ((atAsOfOrLatest != null) && (atAsOfOrLatest.AsOfUtc == asOfUtc))
            {
                throw new InvalidOperationException(
                    "Änderung des In-Inventory-Bestandes zum gleichen Zeitpunkt wird nicht unterstützt.");
            }

            var total = 0;
            if (atAsOfOrLatest != null)
                total = atAsOfOrLatest.Total;

            var quantityAsOf = new QuantityAsOf(change, total + change, asOfUtc);
            UpdateFutures(change, asOfUtc);

            _quantities.Add(quantityAsOf);

            _quantities = _quantities.OrderBy(ks => ks.AsOfUtc).ToList();
        }

        private void UpdateFutures(int change, DateTime asOfUtc)
        {
            foreach (var each in _quantities.Where(p => p.AsOfUtc > asOfUtc))
            {
                each.AddToTotal(change);
            }
        }

        public int GetTotalAsOf(DateTime asOfUtc)
        {
            AssertionConcern.AssertArgumentNotNull(asOfUtc, "As of utc must be provided.");

            var atAsOfOrLatest = FindAtAsOfOrLatestBefore(asOfUtc);

            if (atAsOfOrLatest == null)
                return 0;

            return atAsOfOrLatest.Total;
        }
    }
}