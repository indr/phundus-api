namespace Phundus.Common.Domain.Model
{
    using System;

    public class DomainEvent
    {
        public DomainEvent()
        {
            EventGuid = Guid.NewGuid();
            OccuredOnUtc = DateTime.UtcNow;
        }

        public Guid EventGuid { get; private set; }
        
        public DateTime OccuredOnUtc { get; private set; }
    }
}