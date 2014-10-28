namespace Phundus.Common.Messaging
{
    using Events;

    public interface INotificationProducer
    {
        void Produce(StoredEvent storedEvent);
    }
}