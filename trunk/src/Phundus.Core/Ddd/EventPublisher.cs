namespace Phundus.Ddd
{
    using System;
    using Castle.Windsor;
    using Common.Domain.Model;

    public static class EventPublisher
    {
        private static Func<IEventPublisher> _factory = () => Container.Resolve<IEventPublisher>();

        [Obsolete]
        public static IWindsorContainer Container { get; set; }

        public static void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
        {
            var publisher = _factory();
            publisher.Publish(@event);
        }

        public static void Factory(Func<IEventPublisher> factory)
        {
            _factory = factory;
        }
    }
}