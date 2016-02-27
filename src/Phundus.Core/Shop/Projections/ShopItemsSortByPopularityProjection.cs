namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Orders.Model;

    public class ShopItemsSortByPopularityProjection : ProjectionBase<ShopItemsSortByPopularityProjectionRow>,
        IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(OrderPlaced domainEvent)
        {
            if (domainEvent.Items == null)
                return;

            ProcessItems(domainEvent.Items);
        }

        private void ProcessItems(IEnumerable<OrderEventLine> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            foreach (var item in items)
                ProcessItem(item);
        }

        private void ProcessItem(OrderEventLine line)
        {
            if (line == null) throw new ArgumentNullException("line");
            if (line.ArticleId == Guid.Empty)
                return;

            var from = new DateTime(line.FromUtc.Year, line.FromUtc.Month, 1);
            var to = new DateTime(line.ToUtc.Year, line.ToUtc.Month, 1).AddMonths(1);

            for (var firstOf = from.Date; firstOf < to; firstOf = firstOf.AddMonths(1))
            {
                var month = firstOf.Month;

                var row = Find(line.ArticleId, month) ??
                          new ShopItemsSortByPopularityProjectionRow(line.ArticleId, month);
                row.Value += 1;
                InsertOrUpdate(row);
            }
        }

        private ShopItemsSortByPopularityProjectionRow Find(Guid articleId, int month)
        {
            Session.Flush();
            return QueryOver().Where(p => p.ArticleId == articleId && p.Month == month).SingleOrDefault();
        }
    }

    public class ShopItemsSortByPopularityProjectionRow
    {
        private Guid _articleId;
        private int _month;

        public ShopItemsSortByPopularityProjectionRow(Guid articleId, int month)
        {
            _articleId = articleId;
            _month = month;
        }

        public ShopItemsSortByPopularityProjectionRow()
        {
        }

        public virtual Guid RowId { get; protected set; }

        public virtual Guid ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public virtual int Month
        {
            get { return _month; }
            protected set { _month = value; }
        }

        public virtual int Value { get; set; }
    }
}