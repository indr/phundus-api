namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    public abstract class EventSourcedAggregateRootStepsBase<TAggregateRoot>
        where TAggregateRoot : EventSourcedRootEntity
    {
        private readonly IList<IDomainEvent> _eventStream = new List<IDomainEvent>();

        private int _mutatingEventIdx;

        private TAggregateRoot _sut;

        protected IList<IDomainEvent> EventStream
        {
            get { return _eventStream; }
        }

        protected TAggregateRoot Sut
        {
            get
            {
                if (_sut == null)
                    CreateSut();
                return _sut;
            }
        }

        private void CreateSut()
        {
            var ctor = typeof (TAggregateRoot)
                .GetConstructor(new[] {typeof (IEnumerable<IDomainEvent>), typeof (long)});

            Assert.That(ctor, Is.Not.Null,
                "Constructor of type " + typeof (TAggregateRoot).Name +
                " with parameters IEnumerable<IDomainEvent>, long not found");

            if (ctor != null)
                _sut = (TAggregateRoot) ctor.Invoke(new object[] {_eventStream, 0});
        }

        protected T GetNextExpectedEvent<T>()
        {
            var domainEvent = Sut.MutatingEvents[_mutatingEventIdx++];
            Assert.That(domainEvent, Is.TypeOf<T>());
            Assert.That(domainEvent, Is.Not.Null);

            return (T) domainEvent;
        }
    }

    [Binding]
    public class QuantityInInventorySteps : EventSourcedAggregateRootStepsBase<Stock>
    {
        private ArticleId _articleId;
        private StockId _stockId;

        [Given(@"stock created")]
        public void StockCreated()
        {
            _stockId = new StockId();
            _articleId = new ArticleId(1);
            EventStream.Add(new StockCreated(_stockId.Id, _articleId.Id));
        }

        [When(@"Increase quantity in inventory (.*)")]
        public void WhenIncreaseQuantityInInventory(int p0)
        {
            Sut.IncreaseQuantityInInventory(p0);
        }

        [Then(@"quantity in inventory increased (.*)")]
        public void ThenQuantityInInventoryIncreased(int p0)
        {
            var expected = GetNextExpectedEvent<QuantityInInventoryIncreased>();
            Assert.That(expected.Amount, Is.EqualTo(p0));
        }

        [When(@"I remove (.*) from the inventory")]
        public void WhenIRemoveFromTheInventory(int p0)
        {
            Sut.DecreaseQuantityInInventory(p0);
        }

        [Given(@"quantity in inventory increased (.*)")]
        public void GivenQuantityInInventoryIncreased(int p0)
        {
            EventStream.Add(new QuantityInInventoryIncreased(p0));
        }

        [Then(@"quantity in inventory decreased (.*)")]
        public void ThenQuantityInInventoryDecreased(int p0)
        {
            var expected = GetNextExpectedEvent<QuantityInInventoryDecreased>();
            Assert.That(expected.Amount, Is.EqualTo(p0));
        }
    }
}