namespace Phundus.Core.Dashboard.Querying
{
    using Common;
    using Common.Notifications;

    public class ReadModelUpdater : INotificationHandler
    {
        public IProcessedNotificationTrackerStore ProcessedNotificationTrackerStore { get; set; }

        public IDomainEventHandlerFactory DomainEventHandlerFactory { get; set; }

        public void Handle(Notification notification)
        {
            var storedEventHandlers = DomainEventHandlerFactory.GetDomainEventHandlers();

            foreach (var handler in storedEventHandlers)
                handler.Handle(notification.Event);
        }

        public void UpdateReadModel(INotificationHandler notificationHandler)
        {
            ProcessedNotificationTrackerStore.GetProcessedNotificationTracker(notificationHandler.GetType().Name);
        }
    }
}