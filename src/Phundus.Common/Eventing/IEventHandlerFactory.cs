namespace Phundus.Common.Eventing
{
    public interface IEventHandlerFactory
    {
        ISubscribeTo<TDomainEvent> GetSubscriberForEvent<TDomainEvent>(TDomainEvent @event);

        ISubscribeTo<TDomainEvent>[] GetSubscribersForEvent<TDomainEvent>(TDomainEvent @event);
    }

    
}