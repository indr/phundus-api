using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Inventory
{
    using NHibernate;

    public class Availability
    {
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }

    public class NetStockChangeByDate
    {
        public DateTime Date { get; set; }
        public int Delta { get; set; }
    }


    public class AvailabilityChecker
    {
        private readonly Article _article;
        private ISession _session;

        public AvailabilityChecker(Article article, ISession session)
        {
            _article = article;
            _session = session;
        }

        public bool Check(DateTime start, DateTime end, int amount)
        {
            var netStock = new NetStockCalculator(_article, _session);
            var netStocks = netStock.From(start).To(end);

            return netStocks.All(each => each.Amount >= amount);
        }
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
                result.Add(new Availability { Date = each.Date, Amount = netStock });
            }

            if (result.SingleOrDefault(p => p.Date == DateTime.Today) == null)
                result.Insert(0, new Availability { Date = DateTime.Today, Amount = netStockAtStart });

            return result;
        }

        private IEnumerable<NetStockChangeByDate> GetNetStockChanges(ISession session, DateTime from, DateTime to)
        {

            return session.CreateSQLQuery(
                @"select [Date], sum([Amount]) as [Delta] from (
	select [from] as [Date], 0 - sum(amount) as [Amount] from OrderItem
        inner join [Order] on [Order].Id = [OrderItem].OrderId and ([Order].Status = :pending or [Order].Status = :approved)
        where ArticleId = :id and ([From] >= :start and [From] <= :end)
        group by [from]
	union all
	select dateadd(day, 1, [to]) as [Date], sum(amount) as [Amount] from OrderItem
        inner join [Order] on [Order].Id = [OrderItem].OrderId and ([Order].Status = :pending or [Order].Status = :approved)
        where ArticleId = :id and ([To] >= :start and [To] <= :end)
        group by [to]
) temp
group by [Date]
order by [Date] asc")
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