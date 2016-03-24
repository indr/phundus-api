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
        private readonly ITimePeriodCollection _tpc = new TimePeriodCollection();

        public IList<QuantityPeriod> Intervals
        {
            get { return new ReadOnlyCollection<QuantityPeriod>(_tpc.Cast<QuantityPeriod>().ToList()); }
        }

        public void Add(QuantityPeriod p)
        {
            var intersections = _tpc.IntersectionPeriods(p).Cast<QuantityPeriod>().ToList();
            if (intersections.Count == 0)
            {
                _tpc.Add(p);
                return;
            }
            foreach (var i in intersections)
            {
                var r = p.GetRelation(i);
                switch (r)
                {
                    case PeriodRelation.After:
                        throw new NotImplementedException("After");
                        break;
                    case PeriodRelation.StartTouching:
                        AddPeriod(p.Start, p.End, p.Quantity);
                        break;
                    case PeriodRelation.StartInside:
                        AddPeriod(p.Start, i.End, p.Quantity + i.Quantity);
                        AddPeriod(i.End, p.End, p.Quantity);
                        i.ShrinkEndTo(p.Start);
                        break;
                    case PeriodRelation.InsideStartTouching:
                        AddPeriod(p.Start, p.End, p.Quantity + i.Quantity);
                        i.ShrinkStartTo(p.End);
                        break;
                    case PeriodRelation.EnclosingStartTouching:
                        AddPeriod(i.End, p.End, p.Quantity);
                        i.AddQuantity(p.Quantity);
                        break;
                    case PeriodRelation.Enclosing:
                        AddPeriod(p.Start, i.Start, p.Quantity);
                        AddPeriod(i.End, p.End, p.Quantity);
                        i.AddQuantity(p.Quantity);
                        break;
                    case PeriodRelation.EnclosingEndTouching:
                        AddPeriod(p.Start, i.Start, p.Quantity);
                        i.AddQuantity(p.Quantity);
                        break;
                    case PeriodRelation.ExactMatch:
                        i.AddQuantity(p.Quantity);
                        break;
                    case PeriodRelation.Inside:
                        AddPeriod(i.Start, p.Start, i.Quantity);
                        AddPeriod(p.Start, p.End, p.Quantity + i.Quantity);
                        i.ShrinkStartTo(p.End);
                        break;
                    case PeriodRelation.InsideEndTouching:
                        AddPeriod(p.Start, p.End, p.Quantity + i.Quantity);
                        i.ShrinkEndTo(p.Start);
                        break;
                    case PeriodRelation.EndInside:
                        AddPeriod(p.Start, i.Start, p.Quantity);
                        AddPeriod(i.Start, p.End, p.Quantity + i.Quantity);
                        i.ShrinkStartTo(p.End);
                        break;
                    case PeriodRelation.EndTouching:
                        AddPeriod(p.Start, p.End, p.Quantity);
                        break;
                    case PeriodRelation.Before:
                        throw new NotImplementedException("Before");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void AddPeriod(DateTime start, DateTime end, int quantity)
        {
            var period = new QuantityPeriod(new Period(start, end), quantity);            
            _tpc.Add(period);
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

        public void Add(DateTime startUtc, DateTime endUtc, int quantity)
        {
            Add(new QuantityPeriod(new Period(startUtc, endUtc), quantity));
        }
    }
}