namespace Phundus.Core.Dashboard.Querying
{
    using System.Collections.Generic;
    using Records;

    public interface IEventsQueries : IQueries
    {
        IEnumerable<EventsListViewRecord> FindAll();
    }

    public interface IQueries
    {
    }
}