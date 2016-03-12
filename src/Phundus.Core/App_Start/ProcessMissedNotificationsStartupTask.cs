namespace Phundus
{
    using System;
    using Bootstrap.Extensions.StartupTasks;
    using Common.Commanding;
    using Common.Notifications.Application;

    public class ProcessMissedNotificationsStartupTask : IStartupTask
    {
        private readonly ICommandDispatcher _bus;

        public ProcessMissedNotificationsStartupTask(ICommandDispatcher bus)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            _bus = bus;
        }

        public void Run()
        {
            _bus.Dispatch(new ProcessMissedNotifications());
        }

        public void Reset()
        {
        }
    }
}