namespace Phundus.Common.Messaging
{
    using Events;

    public interface INotificationProducer
    {
        void Produce(IEventStore eventStore, StoredEvent storedEvent);
    }
}