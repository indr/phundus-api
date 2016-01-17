namespace Phundus.Tests
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Ddd;

    public class aggregate_concern_new<TAggregate> : Observes<TAggregate> where TAggregate : class
    {
        protected static IEventPublisher publisher;

        private Cleanup cleanup = () => EventPublisher.Factory(null);

        private Establish ctx = () =>
        {
            publisher = fake.an<IEventPublisher>();

            EventPublisher.Factory(() => publisher);
        };
    }
}