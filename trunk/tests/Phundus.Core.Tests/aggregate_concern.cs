namespace Phundus.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Tests.Eventing;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class aggregate_concern<TAggregate> : Observes<TAggregate> where TAggregate : class
    {
        protected static IEventPublisher publisher;
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;

        private Cleanup cleanup = () =>
            EventPublisher.Factory(null);

        private Establish ctx = () =>
        {            
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");

            publisher = fake.an<IEventPublisher>();            
            EventPublisher.Factory(() => publisher);
        };

        protected static IMethodCallOccurrence published<T>(Expression<Predicate<T>> eventPredicate)
            where T : DomainEvent
        {
            return publisher.received(x =>
                x.Publish(Arg<T>.Matches(eventPredicate)));
        }

        protected static void notPublished<T>() where T : DomainEvent
        {
            publisher.never_received(x =>
                x.Publish(Arg<T>.Is.Anything));
        }
    }
}