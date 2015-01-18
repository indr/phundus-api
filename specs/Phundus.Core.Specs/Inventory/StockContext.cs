namespace Phundus.Core.Specs.Inventory
{
    using System.Collections.Generic;
    using Contexts;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;

    public class StockContext
    {
        private Stock _sut = null;

        public StockContext(PastEvents pastEvents, MutatingEvents mutatingEvents)
        {
            PastEvents = pastEvents;
            MutatingEvents = mutatingEvents;

            InitiatorId = new UserId(10001);
            OrganizationId = new OrganizationId(1001);
            ArticleId = new ArticleId(100001);
            StockId = new StockId("Stock-1");
        }

        public UserId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }

        public ArticleId ArticleId { get; set; }
        public StockId StockId { get; set; }

        public PastEvents PastEvents { get; private set; }
        public MutatingEvents MutatingEvents { get; private set; }

        public Stock Sut
        {
            get
            {
                if (_sut == null)
                    _sut = new Stock(PastEvents.Events, 1);
                return _sut;
            }
        }

        private IDictionary<string, StockData> _stocks = new Dictionary<string, StockData>(); 

        public void AddStock()
        {
            var stockData = new StockData(StockId.Id);
            stockData.OrganizationId = OrganizationId.Id;
            stockData.ArticleId = ArticleId.Id;
            _stocks.Add(stockData.StockId, stockData);
        }

        public StockData GetStock(string stockId)
        {
            StockData result;
            _stocks.TryGetValue(stockId, out result);
            return result;
        }
    }
}