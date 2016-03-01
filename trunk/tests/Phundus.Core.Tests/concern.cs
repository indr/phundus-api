namespace Phundus.Tests
{
    using System;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using Common.Eventing;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        // ReSharper disable StaticFieldInGenericType


        protected static Mock mock = new Mock();

        protected static IEventPublisher publisher;


        // ReSharper restore StaticFieldInGenericType

        protected static Exception caughtException;
        protected static bool catchException;
        private Cleanup cleanup = () => EventPublisher.Factory(null);

        private Establish ctx = () =>
        {
            caughtException = null;
            catchException = false;
            publisher = depends.@on<IEventPublisher>();
            EventPublisher.Factory(() => publisher);
        };

        protected static IMethodCallOccurrence published<T>(Expression<Predicate<T>> eventPredicate)
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