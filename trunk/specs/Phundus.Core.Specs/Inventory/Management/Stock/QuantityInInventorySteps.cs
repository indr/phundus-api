namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    public abstract class EventSourcedAggregateRootStepsBase<TAggregateRoot>
        where TAggregateRoot : EventSourcedRootEntity
    {
        private readonly IList<IDomainEvent> _pastEvents = new List<IDomainEvent>();

        private int _mutatingEventIdx;
        private TAggregateRoot _sut;

        protected IList<IDomainEvent> PastEvents
        {
            get { return _pastEvents; }
        }

        protected TAggregateRoot Sut
        {
            get
            {
                if (_sut != null)
                    return _sut;

                var ctor =
                    typeof (TAggregateRoot).GetConstructor(new[] {typeof (IEnumerable<IDomainEvent>), typeof (long)});

                Assert.That(ctor, Is.Not.Null,
                    "Constructor of type " + typeof (TAggregateRoot).Name +
                    " with parameters IEnumerable<IDomainEvent>, long not found");

                _sut = (TAggregateRoot) ctor.Invoke(new object[] {_pastEvents, 0});
                return _sut;
            }
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
        [Given(@"stock created")]
        public void StockCreated()
        {
            PastEvents.Add(new StockCreated());
        }

        [When(@"I add (.*) to the inventory")]
        public void WhenIAddToTheInventory(int p0)
        {
            // TODO: Execute Command
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
            // TODO: Execute Command
            Sut.DecreaseQuantityInInventory(p0);
        }

        [Given(@"quantity in inventory increased (.*)")]
        public void GivenQuantityInInventoryIncreased(int p0)
        {
            PastEvents.Add(new QuantityInInventoryIncreased(p0));
        }

        [Then(@"quantity in inventory decreased (.*)")]
        public void ThenQuantityInInventoryDecreased(int p0)
        {
            var expected = GetNextExpectedEvent<QuantityInInventoryDecreased>();
            Assert.That(expected.Amount, Is.EqualTo(p0));
        }
    }
}