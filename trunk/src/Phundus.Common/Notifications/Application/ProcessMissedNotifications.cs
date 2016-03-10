namespace Phundus.Common.Notifications.Application
{
    using System;
    using Commanding;

    public class ProcessMissedNotifications : ICommand
    {
    }

    public class ProcessMissedNotificationsHandler : IHandleCommand<ProcessMissedNotifications>
    {
        private readonly INotificationHandler _notificationHandler;

        public ProcessMissedNotificationsHandler(INotificationHandler notificationHandler)
        {
            if (notificationHandler == null) throw new ArgumentNullException("notificationHandler");
            _notificationHandler = notificationHandler;
        }

        public void Handle(ProcessMissedNotifications command)
        {
            _notificationHandler.ProcessMissedNotifications();
        }
    }
}