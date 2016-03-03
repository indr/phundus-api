namespace Phundus.Common.Projecting
{
    using System;
    using Notifications;

    public class ProjectionProcessorNotificationConsumer : INotificationConsumer
    {
        private readonly IProjectionProcessor _projectionProcessor;

        public ProjectionProcessorNotificationConsumer(IProjectionProcessor projectionProcessor)
        {
            if (projectionProcessor == null) throw new ArgumentNullException("projectionProcessor");
            _projectionProcessor = projectionProcessor;
        }

        public void Handle(Notification notification)
        {
            _projectionProcessor.Handle(notification);
        }

        public void ProcessMissedNotifications()
        {
            _projectionProcessor.ProcessMissedNotifications();
        }
    }
}