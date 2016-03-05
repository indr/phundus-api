namespace Phundus.Common.Projecting.Application
{
    using System;
    using Commanding;
    using Domain.Model;
    using Notifications;

    public class UpdateProjection : ICommand
    {
        public UpdateProjection(InitiatorId initiatorId, string projectionTypeName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (projectionTypeName == null) throw new ArgumentNullException("projectionTypeName");

            InitiatorId = initiatorId;
            ProjectionTypeName = projectionTypeName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public string ProjectionTypeName { get; protected set; }
    }

    public class UpdateProjectionHandler : IHandleCommand<UpdateProjection>
    {
        private readonly INotificationToConsumersDispatcher _notificationToConsumersDispatcher;

        public UpdateProjectionHandler(INotificationToConsumersDispatcher notificationToConsumersDispatcher)
        {
            if (notificationToConsumersDispatcher == null) throw new ArgumentNullException("notificationToConsumersDispatcher");
            _notificationToConsumersDispatcher = notificationToConsumersDispatcher;
        }

        public void Handle(UpdateProjection command)
        {
            _notificationToConsumersDispatcher.Update(command.ProjectionTypeName);
        }
    }
}