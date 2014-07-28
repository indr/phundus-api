﻿namespace Phundus.Core.Ddd
{
    public interface IEventHandlerFactory
    {
        ISubscribeTo<TDomainEvent> GetSubscriberForEvent<TDomainEvent>(TDomainEvent @event);
    }
}