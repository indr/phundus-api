namespace Phundus.Common.Eventing
{
    using System.Collections.Generic;
    using Domain.Model;

    public class EventStream
    {
        public EventStream(IList<IDomainEvent> events, int version)
        {
            Events = events;
            Version = version;
        }

        /// <summary>
        /// version of the event stream returned
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// all events in the stream
        /// </summary>
        public IList<IDomainEvent> Events { get; private set; }
    }
}