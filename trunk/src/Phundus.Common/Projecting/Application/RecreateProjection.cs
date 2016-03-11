namespace Phundus.Common.Projecting.Application
{
    using System;    
    using Commanding;
    using Domain.Model;
    using Notifications;

    public class RecreateProjection : AsyncCommand
    {
        public RecreateProjection(InitiatorId initiatorId, string projectionTypeName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (projectionTypeName == null) throw new ArgumentNullException("projectionTypeName");
            InitiatorId = initiatorId;
            ProjectionTypeName = projectionTypeName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public string ProjectionTypeName { get; protected set; }
    }

    public class RecreateProjectionHandler : IHandleCommand<RecreateProjection>
    {
        private readonly IProjectionFactory _projectionFactory;
        private readonly IProcessedNotificationTrackerStore _trackerStore;        

        public RecreateProjectionHandler(IProjectionFactory projectionFactory,
            IProcessedNotificationTrackerStore trackerStore)
        {   
            _projectionFactory = projectionFactory;
            _trackerStore = trackerStore;            
        }

        public void Handle(RecreateProjection command)
        {
            var typeName = command.ProjectionTypeName;
            var projection = _projectionFactory.FindProjection(typeName);

            if (projection == null)
                return;
            
            Recreate(GetType());
        }

        private void Recreate(Type entityType)
        {
            throw new NotImplementedException();
        }
    }
}