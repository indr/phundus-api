namespace Phundus.Common.Events
{
    public class EventStreamId
    {
        public EventStreamId(string streamName, long streamVersion)
        {
            StreamName = streamName;
            StreamVersion = streamVersion;
        }

        public EventStreamId(string streamName) : this(streamName, 0)
        {
        }

        public string StreamName { get; private set; }

        public long StreamVersion { get; private set; }

        public static EventStreamId Empty
        {
            get { return new EventStreamId(null, 0); }
        }
    }
}