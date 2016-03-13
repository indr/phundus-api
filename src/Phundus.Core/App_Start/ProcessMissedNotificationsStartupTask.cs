namespace Phundus
{
    using System;
    using Bootstrap.Extensions.StartupTasks;
    using Common.Commanding;
    using Common.Notifications.Application;

    public class ProcessMissedNotificationsStartupTask : IStartupTask
    {
        private readonly ICommandDispatcher _dispatcher;

        public ProcessMissedNotificationsStartupTask(ICommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Run()
        {
            _dispatcher.Dispatch(new ProcessMissedNotifications());
        }

        public void Reset()
        {
        }
    }
}