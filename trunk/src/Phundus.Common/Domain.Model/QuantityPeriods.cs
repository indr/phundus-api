namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Itenso.TimePeriod;

    public class QuantityPeriods
    {
        private readonly PeriodRelation[] _irrelevants =
        {
            PeriodRelation.Before, PeriodRelation.StartTouching,
            PeriodRelation.EndTouching, PeriodRelation.After
        };

        private readonly ITimePeriodCollection _tpc = new TimePeriodCollection();

        public IList<QuantityPeriod> Intervals
        {
            get { return new ReadOnlyCollection<QuantityPeriod>(_tpc.Cast<QuantityPeriod>().ToList()); }
        }

        public void Add(DateTime startUtc, DateTime endUtc, int quantity)
        {
            Add(new QuantityPeriod(new Period(startUtc, endUtc), quantity));
        }

        public void Add(QuantityPeriod p)
        {
            var intersection = GetSections(p).FirstOrDefault();
            if (intersection == null)
            {
                _tpc.Add(p);
                return;
            }

            var i = intersection;
            var r = p.GetRelation(i);
            switch (r)
            {
                case PeriodRelation.StartInside:
                    var i_e = i.End;
                    i.ShrinkEndTo(p.Start);
                    Add(p.Start, i_e, i.Quantity + p.Quantity);
                    Add(i_e, p.End, p.Quantity);
                    break;
                case PeriodRelation.InsideStartTouching:
                    i.ShrinkStartTo(p.End);
                    Add(p.Start, p.End, p.Quantity + i.Quantity);
                    break;

                case PeriodRelation.EnclosingStartTouching:
                    Add(i.End, p.End, p.Quantity);
                    i.AddQuantity(p.Quantity);
                    break;
                case PeriodRelation.Enclosing:
                    Add(p.Start, i.Start, p.Quantity);
                    Add(i.End, p.End, p.Quantity);
                    i.AddQuantity(p.Quantity);
                    break;
                case PeriodRelation.EnclosingEndTouching:
                    Add(p.Start, i.Start, p.Quantity);
                    i.AddQuantity(p.Quantity);
                    break;
                case PeriodRelation.ExactMatch:
                    i.AddQuantity(p.Quantity);
                    break;
                case PeriodRelation.Inside:
                    var i_s2 = i.Start;
                    i.ShrinkStartTo(p.End);
                    Add(i_s2, p.Start, i.Quantity);
                    Add(p.Start, p.End, p.Quantity + i.Quantity);
                    break;

                case PeriodRelation.InsideEndTouching:
                    i.ShrinkEndTo(p.Start);
                    Add(p.Start, p.End, i.Quantity + p.Quantity);
                    break;
                case PeriodRelation.EndInside:
                    var i_s = i.Start;
                    i.ShrinkStartTo(p.End);
                    Add(i_s, p.End, i.Quantity + p.Quantity);
                    Add(p.Start, i_s, p.Quantity);
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<QuantityPeriod> GetSections(QuantityPeriod p)
        {
            return _tpc.IntersectionPeriods(p)
                    .Cast<QuantityPeriod>()
                    .Where(s => !_irrelevants.Contains(p.GetRelation(s)))
                    .ToList();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(String.Format("QuantityPeriods ({0}):", _tpc.Count));

            foreach (var each in Intervals.OrderBy(p => p.Start).ThenBy(p => p.End))
                sb.AppendLine(each.Start.ToString(CultureInfo.GetCultureInfo("DE-ch")) + " - " +
                              each.End.ToString(CultureInfo.GetCultureInfo("DE-ch")) + " | " + each.Quantity);

            return sb.ToString().Trim();
        }
    }
}