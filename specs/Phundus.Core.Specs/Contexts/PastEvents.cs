namespace Phundus.Core.Specs.Contexts
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Events;
    using Common.Notifications;

    public class PastEvents
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();
        private readonly Container _container;
        private readonly IEventSerializer _serializer;

        public PastEvents(Container container)
        {
            _container = container;
            _serializer = new EventSerializer();
        }

        public IList<IDomainEvent> Events
        {
            get { return _events; }
        }

        public void Add(IDomainEvent e)
        {
            byte[] serialization = _serializer.Serialize(e);
            var deserialized = (IDomainEvent) _serializer.Deserialize(e.GetType(), e.Id, e.OccuredOnUtc, serialization);

            _events.Add(deserialized);
        }

        public void ProjectTo<T>() where T : IDomainEventHandler
        {
            var projection = _container.Resolve<T>();
            foreach (var each in _events)
            {
                projection.Handle(each);
            }
        }
    }
}