namespace Phundus.Common.Eventing.Application
{
    using System;
    using Commanding;
    using Domain.Model;
    using Notifications;

    public class ForceProcessing : AsyncCommand
    {
        public ForceProcessing(InitiatorId initiatorId, string fullName)
        {
            InitiatorId = initiatorId;
            FullName = fullName;
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (fullName == null) throw new ArgumentNullException("fullName");
        }

        public InitiatorId InitiatorId { get; protected set; }
        public string FullName { get; protected set; }
    }

    public class ForceProcessingHandler : IHandleCommand<ForceProcessing>
    {
        private readonly IEventHandlerDispatcher _eventHandlerDispatcher;

        public ForceProcessingHandler(IEventHandlerDispatcher eventHandlerDispatcher)
        {
            _eventHandlerDispatcher = eventHandlerDispatcher;
        }

        public void Handle(ForceProcessing command)
        {
            _eventHandlerDispatcher.Force(command.FullName);
        }
    }
}