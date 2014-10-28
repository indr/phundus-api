namespace Phundus.Common.Domain.Model
{
    using System.Collections.Generic;

    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        private readonly List<IDomainEvent> _mutatingEvents;
        private readonly long _unmutatedVersion;

        protected EventSourcedRootEntity()
        {
            _mutatingEvents = new List<IDomainEvent>();
        }

        public EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, long streamVersion)
            : this()
        {
            foreach (var e in eventStream)
                When(e);

            _unmutatedVersion = streamVersion;
        }

        protected long UnmutatedVersion
        {
            get { return _unmutatedVersion; }
        }

        public IList<IDomainEvent> MutatingEvents
        {
            get { return _mutatingEvents; }
        }

        public long MutatedVersion
        {
            get { return _unmutatedVersion + 1; }
        }

        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            When(e);
        }

        protected abstract void When(IDomainEvent e);
    }
}