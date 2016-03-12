namespace Phundus.Dashboard.Projections
{
    using System.Collections.Generic;
    using Common.Querying;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogData> Query();
    }

    public class EventLogQueries : QueryBase<EventLogData>, IEventLogQueries
    {
        public IEnumerable<EventLogData> Query()
        {
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }
}