namespace Phundus.Dashboard.Queries
{
    using System.Collections.Generic;
    using Projections;

    public interface IEventLogQueries
    {
        IEnumerable<EventLogProjectionRow> FindMostRecent20();
    }
}