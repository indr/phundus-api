namespace Phundus.Dashboard.Queries
{
    using System.Collections.Generic;
    using Projections;
    using Querying;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogProjectionRow> FindMostRecent20();
    }
}