namespace Phundus.Core.Dashboard.Querying.Records
{
    using System;

    public class EventsRecord
    {
        public virtual Guid EventGuid { get; set; }
        public virtual DateTime OccuredOnUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }
    }
}