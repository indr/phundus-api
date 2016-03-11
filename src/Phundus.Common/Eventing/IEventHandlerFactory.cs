namespace Phundus.Common.Eventing
{
    public interface IEventHandlerFactory
    {
        ISubscribeTo FindSubscriber(string typeName);
        ISubscribeTo[] GetSubscribers();
        ISubscribeTo<TDomainEvent> GetSubscriberForEvent<TDomainEvent>(TDomainEvent e);
        ISubscribeTo<TDomainEvent>[] GetSubscribersForEvent<TDomainEvent>(TDomainEvent e);
    }
}