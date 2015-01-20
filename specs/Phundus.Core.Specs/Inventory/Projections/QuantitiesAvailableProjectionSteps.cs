namespace Phundus.Core.Specs.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Port.Adapter.Persistence.View;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantitiesAvailableProjectionSteps
    {
        private readonly Container _container;
        private readonly StockContext _context;
        private IEnumerable<QuantityAvailableData> _quantitiesAvailable;

        public QuantitiesAvailableProjectionSteps(Container container, StockContext context)
        {
            _container = container;
            _context = context;
        }

        private QuantitiesAvailableQueryService GetQuantitiesAvailableQueryService()
        {
            _context.PastEvents.ProjectTo<NHibernateQuantityAvailableProjection>();

            return _container.Resolve<QuantitiesAvailableQueryService>();
        }

        [Given(@"quantity available changed from (.*) of (.*) in (.*)")]
        public void GivenQuantityAvailableChangedFromOfInStock(DateTime fromUtc, int change, string stockId)
        {
            GivenQuantityAvailableChangedFromToOfInStock(fromUtc, DateTime.MaxValue, change, stockId);
        }

        [Given(@"quantity available changed from (.*) to (.*) of (.*) in (.*)")]
        public void GivenQuantityAvailableChangedFromToOfInStock(DateTime fromUtc, DateTime toUtc, int change,
            string stockId)
        {
            _context.PastEvents.Add(new QuantityAvailableChanged(_context.OrganizationId, _context.ArticleId,
                new StockId(stockId),
                new Period(fromUtc, toUtc), change));
        }

        [When(@"I ask for quantities available in stock ""(.*)""")]
        public void WhenIAskForQuantitiesAvailableInStock(string stockId)
        {
            _quantitiesAvailable =
                GetQuantitiesAvailableQueryService()
                    .AllQuantitiesAvailableByStockId(_context.OrganizationId.Id, _context.ArticleId.Id, stockId);
        }

        [Then(@"quantities available data")]
        public void ThenQuantitiesAvailableData(Table table)
        {
            table.CompareToSet(_quantitiesAvailable);
        }
    }
}