namespace Phundus.Dashboard.Projections
{
    using System.Collections.Generic;
    using Common.Querying;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogData> FindMostRecent20();
    }

    public class EventLogQueries : QueryBase<EventLogData>, IEventLogQueries
    {
        public IEnumerable<EventLogData> FindMostRecent20()
        {
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }
}