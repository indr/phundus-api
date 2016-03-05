namespace Phundus.Common.Notifications
{
    using System;

    public class EventConsumerNotificationListener : INotificationHandler
    {
        private readonly INotificationToConsumersDispatcher _notificationToConsumersDispatcher;

        public EventConsumerNotificationListener(INotificationToConsumersDispatcher notificationToConsumersDispatcher)
        {
            if (notificationToConsumersDispatcher == null) throw new ArgumentNullException("notificationToConsumersDispatcher");
            _notificationToConsumersDispatcher = notificationToConsumersDispatcher;
        }

        public void Handle(Notification notification)
        {
            _notificationToConsumersDispatcher.Process(notification);
        }

        public void ProcessMissedNotifications()
        {
            _notificationToConsumersDispatcher.ProcessMissedNotifications();
        }
    }
}