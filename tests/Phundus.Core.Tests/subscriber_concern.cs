namespace Phundus.Core.Tests
{
    using Core.Ddd;
    using Machine.Specifications;

    public abstract class subscriber_concern<TEvent, TSubscriber> : concern<TSubscriber> where TSubscriber : class, ISubscribeTo<TEvent>
    {
        protected static TEvent @event;

        public Because of = () =>
        {
            @event.ShouldNotBeNull();
            sut.Handle(@event);
        };
    }
}