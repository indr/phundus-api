namespace Phundus.Core.Ddd
{
    using Common.Notifications;

    public interface IEventHandlerFactory
    {
        ISubscribeTo<TDomainEvent> GetSubscriberForEvent<TDomainEvent>(TDomainEvent @event);

        ISubscribeTo<TDomainEvent>[] GetSubscribersForEvent<TDomainEvent>(TDomainEvent @event);
    }

    
}