namespace Phundus.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Ddd;
    using Rhino.Mocks;

    public class aggregate_root_concern<TAggregate> : aggregate_concern<TAggregate> where TAggregate : EventSourcedRootEntity
    {
        protected static void mutatingEvent<T>(Expression<Func<T, bool>> predicate)
        {
            var e = sut.MutatingEvents.SingleOrDefault(p => p.GetType() == typeof(T));

            var typed = (T)e;

            typed.ShouldMatch(predicate);
        }
    }

    public class aggregate_concern<TAggregate> : Observes<TAggregate> where TAggregate : class
    {
        protected static IEventPublisher publisher;
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;

        private Cleanup cleanup = () => EventPublisher.Factory(null);

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");

            publisher = fake.an<IEventPublisher>();
            EventPublisher.Factory(() => publisher);
        };

        protected static IMethodCallOccurrence Published<T>(Expression<Predicate<T>> eventPredicate)
            where T : DomainEvent
        {
            return publisher.WasToldTo(x =>
                x.Publish(Arg<T>.Matches(eventPredicate)));
        }

        protected static void NotPublished<T>() where T : DomainEvent
        {
            publisher.WasNotToldTo(x =>
                x.Publish(Arg<T>.Is.Anything));
        }

        
    }
}