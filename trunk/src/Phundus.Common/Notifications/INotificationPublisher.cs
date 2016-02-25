namespace Phundus.Common.Notifications
{
    using Eventing;

    public interface INotificationPublisher
    {
        void PublishNotification(StoredEvent storedEvent);
    }
}