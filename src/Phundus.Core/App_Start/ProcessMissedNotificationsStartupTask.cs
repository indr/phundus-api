namespace Phundus
{
    using System;
    using Bootstrap.Extensions.StartupTasks;
    using Common.Messaging;
    using Common.Notifications.Application;

    public class ProcessMissedNotificationsStartupTask : IStartupTask
    {
        private readonly IBus _bus;

        public ProcessMissedNotificationsStartupTask(IBus bus)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            _bus = bus;
        }

        public void Run()
        {
            _bus.Send(new ProcessMissedNotifications());
        }

        public void Reset()
        {
        }
    }
}