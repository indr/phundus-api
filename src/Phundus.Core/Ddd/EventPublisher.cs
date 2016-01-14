﻿namespace Phundus.Ddd
{
    using Castle.Windsor;
    using Common.Domain.Model;

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