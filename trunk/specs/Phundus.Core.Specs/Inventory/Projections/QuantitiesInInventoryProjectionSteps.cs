namespace Phundus.Core.Specs.Inventory.Projections
{
    using Contexts;
    using Core.Inventory.Application;
    using Core.Inventory.Port.Adapter.Persistence.View;
    using Infrastructure;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class QuantitiesInInventoryProjectionSteps
    {
        private readonly Container _container;
        private readonly PastEvents _pastEvents;
        private QuantitiesInInventoryData _result;

        public QuantitiesInInventoryProjectionSteps(Container container, PastEvents pastEvents)
        {
            _container = container;
            _pastEvents = pastEvents;
        }

        [When(@"I ask for the current quantity in inventory")]
        public void WhenIAskForTheCurrentQuantityInInventory()
        {
            var projection = _container.Resolve<NHibernateQuantitiesInInventoryProjection>();
            foreach (var each in _pastEvents.Events)
            {
                projection.Handle(each);
            }

            var query = _container.Resolve<QuantitiesInInventoryQueryService>();
            _result = query.QuantityDataAsOf(DateTimeProvider.UtcNow);
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int p0)
        {
            Assert.That(_result.Total, Is.EqualTo(p0));
        }
    }
}