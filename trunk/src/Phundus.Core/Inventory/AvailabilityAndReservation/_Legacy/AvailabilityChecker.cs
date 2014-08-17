namespace Phundus.Core.Inventory.AvailabilityAndReservation._Legacy
{
    using System;
    using System.Linq;
    using Articles.Model;
    using NHibernate;

    public class Availability
    {
        public DateTime FromUtc { get; set; }
        public int Amount { get; set; }
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
}