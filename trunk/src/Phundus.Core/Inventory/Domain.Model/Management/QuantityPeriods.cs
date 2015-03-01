namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using NHibernate.Linq;

    public class QuantityPeriods
    {
        private readonly IList<QuantityPeriod> _qps = new List<QuantityPeriod>();

        public QuantityPeriods()
        {
        }

        public QuantityPeriods(QuantityPeriods quantityPeriods)
        {
            foreach (var each in quantityPeriods.Quantities)
                _qps.Add(each);
        }

        public bool IsEmpty
        {
            get { return _qps.Count == 0; }
        }

        public ICollection<QuantityPeriod> Quantities
        {
            get { return _qps; }
        }

        public ICollection<QuantityAsOf> GetQuantityAsOf()
        {
            var result = new List<QuantityAsOf>();
            int total = 0;
            QuantityAsOf last = null;
            foreach (var each in _qps.OrderBy(p => p.FromUtc))
            {
                total = each.Quantity + total;
                if ((last != null) && (last.AsOfUtc == each.FromUtc))
                    last.AddChange(each.Quantity);
                else
                {
                    last = new QuantityAsOf(each.Quantity, total, each.FromUtc);
                    result.Add(last);
                }
            }
            return result;
        }

        public void Add(Period period, int quantity)
        {
            _qps.Add(new QuantityPeriod(period, quantity));
        }

        public void Sub(Period period, int quantity)
        {
            Add(period, quantity * -1);
        }

        public QuantityPeriods Sub(QuantityPeriods other)
        {
            var result = new QuantityPeriods();
            _qps.ForEach(e => result.Add(e.Period, e.Quantity));
            other._qps.ForEach(e => result.Sub(e.Period, e.Quantity));

            return result;
        }

        public int QuantityAsOf(DateTime asOfUtc)
        {
            return FindInPeriod(asOfUtc).Sum(s => s.Quantity);
        }

        public bool HasQuantityAsOf(DateTime asOfUtc, int quantity)
        {
            return FindInPeriod(asOfUtc).Sum(s => s.Quantity) >= quantity;
        }

        private IEnumerable<QuantityPeriod> FindInPeriod(DateTime asOfUtc)
        {
            return _qps.Where(p => p.IsInPeriod(asOfUtc));
        }

        public override string ToString()
        {
            var result = String.Format("QuantityPeriods [IsEmpty={0}, Count={1}", IsEmpty, _qps.Count);

            result += ", Qps=[\n";
            for(var idx = 0; idx < 10 && idx < _qps.Count; idx++)
                result += String.Format("[Period={0}, Quantity={1}]\n", _qps[idx].Period, _qps[idx].Quantity);

            return result + "]]";
        }
    }
}