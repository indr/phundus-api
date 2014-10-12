﻿namespace Phundus.Common.Events
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface IEventStore
    {
        void Append(DomainEvent domainEvent);
        IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        long CountStoredEvents();
        DomainEvent ToDomainEvent(StoredEvent storedEvent);
    }
}