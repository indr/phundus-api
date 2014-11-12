namespace Phundus.Core.Specs.Inventory.Projections
{
    using System.Collections.Generic;
    using Contexts;
    using Core.Inventory.Application;
    using Core.Inventory.Application.Data;
    using Core.Inventory.Port.Adapter.Persistence.View;
    using Infrastructure;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class QuantitiesInInventoryProjectionSteps
    {
        private readonly Container _container;
        private readonly PastEvents _pastEvents;
        private QuantityInInventoryData _result;
        private IEnumerable<QuantityInInventoryData> _results;

        public QuantitiesInInventoryProjectionSteps(Container container, PastEvents pastEvents)
        {
            _container = container;
            _pastEvents = pastEvents;
        }

        private QuantitiesInInventoryQueryService GetQuantitiesInInventoryQueryService()
        {
            _pastEvents.ProjectTo<NHibernateQuantityInInventoryProjection>();

            var query = _container.Resolve<QuantitiesInInventoryQueryService>();
            return query;
        }

        [When(@"I ask for the current quantity in inventory")]
        public void WhenIAskForTheCurrentQuantityInInventory()
        {
            var query = GetQuantitiesInInventoryQueryService();
            _result = query.QuantityDataAsOf(DateTimeProvider.UtcNow);
        }

        [When(@"I ask for all quantities in inventory")]
        public void WhenIAskForAllQuantitiesInInventory()
        {
            var query = GetQuantitiesInInventoryQueryService();
            _results = query.AllQuantitiesInInventoryByArticleId();
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int p0)
        {
            Assert.That(_result.Total, Is.EqualTo(p0));
        }

        [Then(@"the quantities in inventory should be")]
        public void ThenTheQuantitiesInInventoryShouldBe(Table table)
        {
            table.CompareToSet(_results);
        }
    }
}