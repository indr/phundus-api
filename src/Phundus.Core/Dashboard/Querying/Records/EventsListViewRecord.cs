namespace Phundus.Core.Dashboard.Querying.Records
{
    using System;

    public class EventsListViewRecord
    {
        public virtual Guid EventGuid { get; set; }
        public virtual DateTime OccuredOnUtc { get; set; }
        public virtual string Name { get; set; }
    }
}