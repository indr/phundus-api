namespace Phundus.Core.Ddd
{
    using System.Collections.Generic;

    public interface IStoredEventRepository
    {
        IEnumerable<StoredEvent> FindAll();
        void Add(StoredEvent storedEvent);
    }
}