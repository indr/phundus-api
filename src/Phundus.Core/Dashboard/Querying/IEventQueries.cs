namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Records;

    public interface IEventQueries
    {
        IEnumerable<EventsListViewRecord> FindAll();
    }
}