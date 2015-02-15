namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;

    public class QuantityPeriods
    {
        private readonly IList<QuantityPeriod> _qps = new List<QuantityPeriod>();

        public bool IsEmpty { get { return _qps.Count == 0; } }

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

        public void Add(QuantityPeriod qp)
        {
            _qps.Add(qp);
        }

        private void Sub(QuantityPeriod qp)
        {
            _qps.Add(new QuantityPeriod(qp.Period, qp.Quantity * -1));
        }
        
        public QuantityPeriods Sub(QuantityPeriods other)
        {
            var result = new QuantityPeriods();
            this._qps.ForEach(result.Add);
            other._qps.ForEach(result.Sub);

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
            return String.Format("QuantityPeriods [IsEmpty={0}, Count={1}]", IsEmpty, _qps.Count);
        }


        
    }
}