namespace Phundus.Common.Projecting
{
    using System;
    using Castle.Core.Internal;
    using Notifications;

    public interface IProjectionProcessor
    {
        void Update(string typeName);
        void Handle(Notification notification);
        void ProcessMissedNotifications();
    }

    public class ProjectionProcessor : IProjectionProcessor
    {
        private static readonly object Lock = new object();
        private readonly IProjectionFactory _projectionFactory;
        private readonly IProjectionUpdater _projectionUpdater;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public ProjectionProcessor(IProjectionFactory projectionFactory, IProjectionUpdater projectionUpdater,
            IProcessedNotificationTrackerStore trackerStore)
        {
            if (projectionFactory == null) throw new ArgumentNullException("projectionFactory");
            if (projectionUpdater == null) throw new ArgumentNullException("projectionUpdater");
            if (trackerStore == null) throw new ArgumentNullException("trackerStore");
            _projectionFactory = projectionFactory;
            _projectionUpdater = projectionUpdater;
            _trackerStore = trackerStore;
        }

        public void Update(string typeName)
        {
            var projection = _projectionFactory.FindProjection(typeName);
            if (projection == null)
                return;

            UpdateProjection(projection);
        }

        public void Handle(Notification notification)
        {
            UpdateProjections();
        }

        public void ProcessMissedNotifications()
        {
            UpdateProjections();
        }

        private void UpdateProjections()
        {
            lock (Lock)
            {
                var projections = _projectionFactory.GetProjections();
                projections.ForEach(UpdateProjection);
            }
        }

        private void UpdateProjection(IProjection projection)
        {
            lock (Lock)
            {
                try
                {
                    var done = false;
                    while (!done)
                    {
                        done = _projectionUpdater.Update(projection);
                    }
                }
                catch (Exception ex)
                {
                    _trackerStore.TrackException(projection.GetType().FullName, ex);
                }
            }
        }
    }
}