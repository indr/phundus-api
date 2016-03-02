namespace Phundus.Common.Projecting.Application
{
    using System;
    using Castle.MicroKernel;
    using Castle.Transactions;
    using Commanding;
    using Domain.Model;
    using Notifications;

    public class ResetProjection : ICommand
    {
        public ResetProjection(InitiatorId initiatorId, string projectionTypeName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (projectionTypeName == null) throw new ArgumentNullException("projectionTypeName");

            InitiatorId = initiatorId;
            ProjectionTypeName = projectionTypeName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public string ProjectionTypeName { get; protected set; }
    }

    public class ResetProjectionHandler : IHandleCommand<ResetProjection>
    {
        private readonly IProjectionFactory _projectionFactory;
        private readonly IProcessedNotificationTrackerStore _trackerStore;
        private IProjection _projection;
        private string _typeName;

        public ResetProjectionHandler(IProcessedNotificationTrackerStore trackerStore,
            IProjectionFactory projectionFactory)
        {
            if (trackerStore == null) throw new ArgumentNullException("trackerStore");
            if (projectionFactory == null) throw new ArgumentNullException("projectionFactory");
            _trackerStore = trackerStore;
            _projectionFactory = projectionFactory;
        }

        [Transaction]
        public void Handle(ResetProjection command)
        {
            _typeName = command.ProjectionTypeName;

            if (FindProjection())
            {
                ResetProjection();
                ResetTracker();
            }
            else
            {
                DeleteTracker();
            }
        }

        private bool FindProjection()
        {
            try
            {
                _projection = _projectionFactory.GetProjection(_typeName);
            }
            catch (ComponentNotFoundException)
            {
                _projection = null;
            }

            return _projection != null;
        }

        private void DeleteTracker()
        {
            _trackerStore.DeleteTracker(_typeName);
        }

        private void ResetProjection()
        {
            _projection.Reset();
        }

        private void ResetTracker()
        {
            var tracker = _trackerStore.GetProcessedNotificationTracker(_typeName);
            if (tracker == null)
                return;
            _trackerStore.TrackMostRecentProcessedNotificationId(tracker, 0);
        }
    }
}