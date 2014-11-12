namespace Phundus.Core.Specs.Contexts
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;

    public class PastEvents
    {
        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();
        private readonly Container _container;

        public PastEvents(Container container)
        {
            _container = container;
        }

        public IList<IDomainEvent> Events
        {
            get { return _events; }
        }

        public void Add(IDomainEvent e)
        {
            _events.Add(e);
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