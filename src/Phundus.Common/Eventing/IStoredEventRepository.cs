﻿namespace Phundus.Common.Eventing
{
    using System;
    using System.Collections.Generic;

    public interface IStoredEventRepository
    {
        void Append(StoredEvent storedEvent);
        void Append(IEnumerable<StoredEvent> storedEvents, Guid aggregateId, int? expectedVersion);
        IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        long CountStoredEvents();
        long GetMaxNotificationId();
        IEnumerable<StoredEvent> GetStoredEvents(Guid aggregateId);
    }
}