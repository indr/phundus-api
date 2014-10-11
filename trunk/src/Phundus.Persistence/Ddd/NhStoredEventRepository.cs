namespace Phundus.Persistence.Ddd
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Core.Ddd;

    public class NhStoredEventRepository : NhRepositoryBase<StoredEvent>, IStoredEventRepository
    {
        [Transaction]
        public IEnumerable<StoredEvent> FindAll()
        {
            var query = from se in Entities select se;
            return query.ToList();
        }

        public new void Add(StoredEvent storedEvent)
        {
            Session.Save(storedEvent);
        }
    }
}