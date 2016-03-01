namespace Phundus.Common.Eventing
{
    using System;
    using Domain.Model;

    public static class EventPublisher
    {
        private static Func<IEventPublisher> _factory;

        public static void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
        {
            if (_factory == null)
                throw new InvalidOperationException("You need to provide a factory to the static EventPublisher class.");
            var publisher = _factory();
            publisher.Publish(@event);
        }

        public static void Factory(Func<IEventPublisher> factory)
        {
            _factory = factory;
        }
    }
}