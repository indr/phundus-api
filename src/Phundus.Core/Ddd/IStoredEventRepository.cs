namespace Phundus.Core.Ddd
{
    using System.Collections.Generic;
    using Common.Events;

    public interface IStoredEventRepository
    {
        void Append(StoredEvent storedEvent);
        IEnumerable<StoredEvent> AllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        long CountStoredEvents();
        long GetMaxNotificationId();
    }
}