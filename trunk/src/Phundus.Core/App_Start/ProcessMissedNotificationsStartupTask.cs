namespace Phundus
{
    using System;
    using Bootstrap.Extensions.StartupTasks;
    using Common.Notifications;

    public class ProcessMissedNotificationsStartupTask : IStartupTask
    {
        private INotificationConsumer _notificationConsumer;

        public ProcessMissedNotificationsStartupTask(INotificationConsumer notificationConsumer)
        {
            if (notificationConsumer == null) throw new ArgumentNullException("notificationConsumer");

            _notificationConsumer = notificationConsumer;
        }

        public void Run()
        {
            //_notificationConsumer.ProcessMissedNotifications();
            _notificationConsumer = null;
        }

        public void Reset()
        {
        }
    }
}