namespace Phundus.Tests
{
    using Machine.Specifications;
    using Phundus.Ddd;

    public abstract class aggregate_concern<TAggregate> : developwithpassion.specifications.rhinomocks.Observes
    {
        // ReSharper disable StaticFieldInGenericType

        protected static IEventPublisher publisher;

        protected static TAggregate sut;

        // ReSharper restore StaticFieldInGenericType

        private Cleanup cleanup = () => EventPublisher.Factory(null);

        private Establish ctx = () =>
        {
            publisher = fake.an<IEventPublisher>();
            EventPublisher.Factory(() => publisher);
        };
    }
}