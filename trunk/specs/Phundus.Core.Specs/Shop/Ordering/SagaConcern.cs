namespace Phundus.Core.Specs.Shop.Ordering
{
    using System.Linq;
    using Common.Domain.Model;
    using Contexts;
    using NUnit.Framework;

    public class SagaConcern<TSaga> where TSaga : ISaga, new()
    {
        protected SagaConcern(PastEvents pastEvents)
        {
            PastEvents = pastEvents;

            Saga = new TSaga();

            AssertUndispatchedCommandCounter = 0;
        }

        protected TSaga Saga { get; set; }

        protected PastEvents PastEvents { get; set; }

        protected void Transition(IDomainEvent evnt)
        {
            foreach (var each in PastEvents.Events)
                Saga.Transition(each);

            Saga.ClearUncommittedEvents();
            Saga.ClearUndispatchedCommands();

            Saga.Transition(evnt);
        }

        protected TCommand AssertUndispatchedCommand<TCommand>()
        {
            return AssertUndispatchedCommand<TCommand>(default(TCommand));
        }

        private int AssertUndispatchedCommandCounter = 0;

        protected TCommand AssertUndispatchedCommand<TCommand>(TCommand command)
        {
            Assert.That(Saga.UndispatchedCommands.Count, Is.GreaterThan(AssertUndispatchedCommandCounter));
            var actual = Saga.UndispatchedCommands.ToList()[AssertUndispatchedCommandCounter++];
            Assert.That(actual, Is.InstanceOf<TCommand>());
            return (TCommand) actual;
        }

        protected void AssertNoUndispatchedCommands()
        {
            Assert.That(Saga.UndispatchedCommands.Count, Is.EqualTo(0));
        }
    }
}