namespace Phundus.Common.Projecting.Application
{
    using System;
    using Castle.Core.Logging;
    using Commanding;
    using Domain.Model;

    public class ForceProcessProjection : ICommand
    {
        public ForceProcessProjection(InitiatorId initiatorId, string projectionTypeName)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (projectionTypeName == null) throw new ArgumentNullException("projectionTypeName");

            InitiatorId = initiatorId;
            ProjectionTypeName = projectionTypeName;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public string ProjectionTypeName { get; protected set; }
    }

    public class ForceProcessProjectionHandler : IHandleCommand<ForceProcessProjection>
    {
        private readonly IProjectionProcessor _projectionProcessor;        

        public ForceProcessProjectionHandler(IProjectionProcessor projectionProcessor)
        {
            if (projectionProcessor == null) throw new ArgumentNullException("projectionProcessor");

            _projectionProcessor = projectionProcessor;
        }

        public void Handle(ForceProcessProjection command)
        {
            _projectionProcessor.Force(command.ProjectionTypeName);
        }
    }
}