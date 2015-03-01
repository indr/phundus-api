namespace Phundus.Common.Domain.Model
{
    using System.Collections.Generic;

    public abstract class EventSourcedEntity
    {
        private IList<IDomainEvent> _mutatingEvents = new List<IDomainEvent>();

        public IList<IDomainEvent> MutatingEvents
        {
            get { return _mutatingEvents; }
            set { _mutatingEvents = value; }
        }

        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            When(e);
        }

        protected abstract void When(IDomainEvent e);
    }
}