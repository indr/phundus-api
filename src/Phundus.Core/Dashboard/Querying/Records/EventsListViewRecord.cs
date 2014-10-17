namespace Phundus.Core.Dashboard.Querying.Records
{
    using System;

    public class EventsListViewRecord
    {
        public virtual long EventId { get; set; }
        public virtual DateTime OccuredOnUtc { get; set; }
        public virtual string Name { get; set; }
    }
}