﻿namespace Phundus.Common.Events
{
    using System;

    public class StoredEvent : StoredEventBase
    {
        protected StoredEvent()
        {
        }

        public StoredEvent(Guid eventGuid, DateTime occuredOnUtc, string typeName, byte[] serialization,
            string streamName, long streamVersion)
            : base(eventGuid, occuredOnUtc, typeName, serialization, streamName, streamVersion)
        {
        }
    }
}