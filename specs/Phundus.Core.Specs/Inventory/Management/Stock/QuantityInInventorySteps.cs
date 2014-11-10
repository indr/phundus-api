namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Queries;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Supports;
    using TechTalk.SpecFlow;

    public abstract class EventSourcedAggregateRootStepsBase
    {
        private readonly IList<IDomainEvent> _eventStream = new List<IDomainEvent>();
        protected IList<IDomainEvent> MutatingEvents = new List<IDomainEvent>();
        private int _mutatingEventIdx;

        protected IList<IDomainEvent> EventStream
        {
            get { return _eventStream; }
        }

        protected T GetNextExpectedEvent<T>()
        {
            Assert.That(MutatingEvents.Count, Is.GreaterThan(_mutatingEventIdx), "Expected more mutating events");

            var domainEvent = MutatingEvents[_mutatingEventIdx++];
            Assert.That(domainEvent, Is.TypeOf<T>());
            Assert.That(domainEvent, Is.Not.Null);

            return (T) domainEvent;
        }
    }

    [Binding]
    public class QuantityInInventorySteps : EventSourcedAggregateRootStepsBase
    {
        private readonly Container _container;
        private ArticleId _articleId;
        private StockId _stockId;

        public QuantityInInventorySteps(Container container)
        {
            _container = container;
            
            var repository = _container.Depend.On<IStockRepository>();

            repository.Expect(
                x => x.Get(Arg<OrganizationId>.Is.Anything, Arg<ArticleId>.Is.Anything, Arg<StockId>.Is.Anything))
                .WhenCalled(a => a.ReturnValue = new Stock(EventStream, 1)).Return(null);

            repository.Expect(x => x.Save(Arg<Stock>.Is.NotNull))
                .WhenCalled(a => MutatingEvents = ((Stock) a.Arguments[0]).MutatingEvents);
        }

        [Given(@"stock created (.*)")]
        public void StockCreated(string stockId)
        {
            _stockId = new StockId(stockId);
            _articleId = new ArticleId(1);
            EventStream.Add(new StockCreated(_stockId.Id, _articleId.Id));
        }

        [When(@"Increase quantity in inventory (.*)")]
        public void WhenIncreaseQuantityInInventory(int quantity)
        {
            _container.Resolve<IncreaseQuantityInInventoryHandler>()
                .Handle(new IncreaseQuantityInInventory(1, 2, _articleId.Id, _stockId.Id, quantity));
        }

        [Then(@"quantity in inventory increased (.*)")]
        public void ThenQuantityInInventoryIncreased(int quantity)
        {
            var expected = GetNextExpectedEvent<QuantityInInventoryIncreased>();
            Assert.That(expected.StockId, Is.EqualTo(_stockId.Id));
            Assert.That(expected.Quantity, Is.EqualTo(quantity));
        }

        [When(@"I remove (.*) from the inventory")]
        public void WhenIRemoveFromTheInventory(int quantity)
        {
            _container.Resolve<DecreaseQuantityInInventoryHandler>()
                .Handle(new DecreaseQuantityInInventory(1, 2, _articleId.Id, _stockId.Id, quantity));
        }

        [Given(@"quantity in inventory increased (.*)")]
        public void GivenQuantityInInventoryIncreased(int quantity)
        {
            EventStream.Add(new QuantityInInventoryIncreased(_stockId.Id, quantity));
        }

        [Then(@"quantity in inventory decreased (.*)")]
        public void ThenQuantityInInventoryDecreased(int quantity)
        {
            var expected = GetNextExpectedEvent<QuantityInInventoryDecreased>();
            Assert.That(expected.StockId, Is.EqualTo(_stockId.Id));
            Assert.That(expected.Quantity, Is.EqualTo(quantity));
        }
    }
}