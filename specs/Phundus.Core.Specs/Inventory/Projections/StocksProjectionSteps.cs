namespace Phundus.Core.Specs.Inventory.Projections
{
    using System.Collections.Generic;
    using Contexts;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Port.Adapter.Persistence.View;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class StocksProjectionSteps
    {
        private readonly PastEvents _pastEvents;
        private Container _container;
        private IEnumerable<StockData> _results;

        public StocksProjectionSteps(Container container, PastEvents pastEvents)
        {
            _container = container;
            _pastEvents = pastEvents;
        }

        [When(@"I ask for all stocks of article (.*)")]
        public void WhenIAskForAllStocks(int articleId)
        {
            _pastEvents.ProjectTo<NHibernateStocksProjection>();

            var query = _container.Resolve<StocksQueryService>();
            _results = query.AllStocksByArticleId(articleId);
        }

        [Then(@"the stocks should be")]
        public void ThenTheStocksShouldBe(Table table)
        {
            table.CompareToSet(_results);
        }
    }
}