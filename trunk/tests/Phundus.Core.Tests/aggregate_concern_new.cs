namespace Phundus.Tests
{
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Ddd;

    public class aggregate_concern_new<TAggregate> : Observes<TAggregate> where TAggregate : class
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
    }
}