namespace Phundus.Common.Events
{
    public class EventStreamId
    {
        public EventStreamId(string streamName, int streamVersion)
        {
            StreamName = streamName;
            StreamVersion = streamVersion;
        }

        public string StreamName { get; private set; }

        public long StreamVersion { get; private set; }

        public static EventStreamId Empty
        {
            get
            {
                return new EventStreamId(null, 0);
            }
        }
    }
}