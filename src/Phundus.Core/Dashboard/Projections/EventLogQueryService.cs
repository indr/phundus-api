namespace Phundus.Dashboard.Projections
{
    using System.Collections.Generic;
    using Common.Querying;

    public interface IEventLogQueryService
    {
        IEnumerable<EventLogData> Query();
    }

    public class EventLogQueryService : QueryServiceBase<EventLogData>, IEventLogQueryService
    {
        public IEnumerable<EventLogData> Query()
        {
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }
}