namespace Phundus.Core.Dashboard.Application.Data
{
    using System;

    public class ActivityData
    {
        public virtual Guid EventGuid { get; set; }
        public virtual DateTime OccuredOnUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }
    }
}