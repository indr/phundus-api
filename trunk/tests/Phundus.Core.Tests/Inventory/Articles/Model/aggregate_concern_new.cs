namespace Phundus.Tests.Inventory.Articles.Model
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Ddd;

    public class aggregate_concern_new<TAggregate> : Observes<TAggregate> where TAggregate : class
    {
        private Establish ctx = () =>
        {
            publisher = fake.an<IEventPublisher>();

            EventPublisher.Factory(() => publisher);
        };

        protected static IEventPublisher publisher;
    }
}