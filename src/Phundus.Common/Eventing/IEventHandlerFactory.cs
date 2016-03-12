namespace Phundus.Common.Eventing
{
    public interface IEventHandlerFactory
    {
        ISubscribeTo GetSubscriber(string typeName);
        ISubscribeTo[] GetSubscribers();

        ISubscribeTo[] GetSubscribersForEventNonGeneric(object e);
        ISubscribeTo<TDomainEvent>[] GetSubscribersForEvent<TDomainEvent>(TDomainEvent e);
    }
}