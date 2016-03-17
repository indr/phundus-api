namespace Phundus.Dashboard.Application
{
    using System.Collections.Generic;
    using Common.Querying;
    using Projections;

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