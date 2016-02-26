﻿namespace Phundus.Common.Eventing
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface IEventStore
    {
        void Append(DomainEvent domainEvent);
        IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        long CountStoredEvents();
        long GetMaxNotificationId();
        DomainEvent ToDomainEvent(StoredEvent storedEvent);

        EventStream LoadEventStream(GuidIdentity id);
    }
}