namespace Phundus.Dashboard.Projections
{
    using System;

    public class EventLogProjectionRow
    {
        public virtual Guid EventGuid { get; set; }
        public virtual DateTime OccuredOnUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }
    }
}