namespace Phundus.Tests
{
    using Machine.Specifications;
    using Phundus.Ddd;

    public abstract class subscriber_concern<TEvent, TSubscriber> : concern<TSubscriber>
        where TSubscriber : class, ISubscribeTo<TEvent>
    {
        protected static TEvent @event;

        public Because of = () =>
        {
            @event.ShouldNotBeNull();
            sut.Handle(@event);
        };
    }
}