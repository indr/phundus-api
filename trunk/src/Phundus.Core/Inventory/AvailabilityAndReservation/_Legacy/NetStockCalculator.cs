namespace Phundus.Core.Inventory.AvailabilityAndReservation._Legacy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles.Model;
    using NHibernate;
    using NHibernate.Transform;
    using Shop.Orders.Model;

    public class NetStockChangeByDate
    {
        public DateTime FromUtc { get; set; }
        public int Delta { get; set; }
    }

    public class NetStockCalculator
    {
        private readonly Article _article;
        private DateTime _start;
        private DateTime _to;
        private ISession _session;

        public NetStockCalculator(Article article, ISession session)
        {
            _article = article;
            _start = DateTime.Today;
            _to = DateTime.Today.AddYears(1);
            _session = session;
        }

        public NetStockCalculator From(DateTime start)
        {
            _start = start;
            return this;
        }

        public IList<Availability> To(DateTime end)
        {
            _to = end;
            return Compute();
        }

        private int At(DateTime day)
        {
            var grossStock = _article.GrossStock;
            var netStockChanges = GetNetStockChanges(_session, new DateTime(2000, 1, 1), day.AddDays(-1));
            return grossStock + netStockChanges.Sum(s => s.Delta);
        }

        private IList<Availability> Compute()
        {
            var netStockChanges = GetNetStockChanges(_session, _start, _to);
            var result = new List<Availability>();
            var netStockAtStart = At(_start);
            var netStock = netStockAtStart;
            foreach (var each in netStockChanges)
            {
                netStock = netStock + each.Delta;
                result.Add(new Availability { FromUtc = each.FromUtc, Amount = netStock });
            }

            if (result.SingleOrDefault(p => p.FromUtc.Date == DateTime.UtcNow.Date) == null)
                result.Insert(0, new Availability { FromUtc = DateTime.UtcNow.Date, Amount = netStockAtStart });

            return result;
        }

        private IEnumerable<NetStockChangeByDate> GetNetStockChanges(ISession session, DateTime from, DateTime to)
        {

            return session.CreateSQLQuery(
                @"select [FromUtc], sum([Amount]) as [Delta] from (
	select [fromUtc] as [FromUtc], 0 - sum(amount) as [Amount] from OrderItem
        inner join [Order] on [Order].Id = [OrderItem].OrderId and ([Order].Status = :pending or [Order].Status = :approved)
        where ArticleId = :id and ([FromUtc] >= :start and [FromUtc] <= :end)
        group by [FromUtc]
	union all
	select dateadd(second, 1, [toUtc]) as [FromUtc], sum(amount) as [Amount] from OrderItem
        inner join [Order] on [Order].Id = [OrderItem].OrderId and ([Order].Status = :pending or [Order].Status = :approved)
        where ArticleId = :id and ([ToUtc] >= :start and [ToUtc] <= :end)
        group by [toUtc]
) temp
group by [FromUtc]
order by [FromUtc] asc")
                .SetParameter("id", _article.Id)
                .SetParameter("pending", OrderStatus.Pending)
                .SetParameter("approved", OrderStatus.Approved)
                .SetParameter("start", from)
                .SetParameter("end", to)
                .SetResultTransformer(Transformers.AliasToBean(typeof(NetStockChangeByDate)))
                .List<NetStockChangeByDate>();
        }
    }
}