namespace Phundus.Common.Events
{
    using System;

    public class SagaStoredEvent : StoredEventBase
    {
        protected SagaStoredEvent()
        {
        }

        public SagaStoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization,
            string streamName, long streamVersion)
            : base(eventGuid, occuredOnUtc, typeName, serialization, streamName, streamVersion)
        {
        }
    }
}