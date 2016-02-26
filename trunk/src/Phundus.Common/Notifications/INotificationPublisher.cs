namespace Phundus.Common.Notifications
{
    using System.Collections.Generic;
    using Eventing;

    public interface INotificationPublisher
    {
        void PublishNotification(StoredEvent storedEvent);
        void PublishNotification(IEnumerable<StoredEvent> storedEvents);
    }
}