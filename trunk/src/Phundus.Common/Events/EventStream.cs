namespace Phundus.Common.Events
{
    using System.Collections.Generic;
    using Domain.Model;

    public class EventStream
    {
        public EventStream(IList<DomainEvent> events, long version)
        {
            Events = events;
            Version = version;
        }

        public IList<DomainEvent> Events { get; private set; }

        public long Version { get; private set; }
    }
}