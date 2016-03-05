﻿namespace Phundus.Common.Projecting.Application
{
    using System;
    using Commanding;
    using Domain.Model;

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
        private readonly IProjectionProcessor _projectionProcessor;

        public UpdateProjectionHandler(IProjectionProcessor projectionProcessor)
        {
            if (projectionProcessor == null) throw new ArgumentNullException("projectionProcessor");
            _projectionProcessor = projectionProcessor;
        }

        public void Handle(UpdateProjection command)
        {
            _projectionProcessor.Update(command.ProjectionTypeName);
        }
    }
}