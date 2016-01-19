namespace Phundus
{
    using System;
    using Bootstrap.Extensions.StartupTasks;
    using Common.Notifications;

    public class ProcessMissedNotificationsStartupTask : IStartupTask
    {
        private INotificationHandler _notificationHandler;

        public ProcessMissedNotificationsStartupTask(INotificationHandler notificationHandler)
        {
            if (notificationHandler == null) throw new ArgumentNullException("notificationHandler");
            _notificationHandler = notificationHandler;
        }

        public void Run()
        {
            _notificationHandler.ProcessMissedNotifications();
            _notificationHandler = null;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}