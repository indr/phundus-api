namespace Phundus.Dashboard.Projections
{
    using System.Collections.Generic;
    using Common.Querying;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogData> Query();
    }

    public class EventLogQueries : QueryServiceBase<EventLogData>, IEventLogQueries
    {
        public IEnumerable<EventLogData> Query()
        {
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }
}