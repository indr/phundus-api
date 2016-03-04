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

    public class ResetProjectionHandler : ProjectionHandlerBase, IHandleCommand<ResetProjection>
    {
        private readonly IProjectionFactory _projectionFactory;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public ResetProjectionHandler(IProjectionFactory projectionFactory,
            IProcessedNotificationTrackerStore trackerStore)
        {
            if (projectionFactory == null) throw new ArgumentNullException("projectionFactory");
            if (trackerStore == null) throw new ArgumentNullException("trackerStore");
            _projectionFactory = projectionFactory;
            _trackerStore = trackerStore;
        }

        [Transaction]
        public void Handle(ResetProjection command)
        {
            var typeName = command.ProjectionTypeName;
            var projection = _projectionFactory.FindProjection(typeName);

            if (projection == null)
            {
                _trackerStore.DeleteTracker(typeName);
                return;
            }

            if (!projection.CanReset)
            {
                return;
            }

            projection.Reset();
            _trackerStore.ResetTracker(typeName);
        }
    }

    public class ProjectionHandlerBase
    {
    }
}