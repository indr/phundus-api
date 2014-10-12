namespace Phundus.Core.Ddd
{
    using Castle.Windsor;

    public static class EventPublisher
    {
        public static IWindsorContainer Container { get; set; }

        public static void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
        {
            var publisher = Container.Resolve<IEventPublisher>();
            publisher.Publish(@event);
        }
    }
}