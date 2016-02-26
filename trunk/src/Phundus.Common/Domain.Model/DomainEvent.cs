namespace Phundus.Common.Domain.Model
{
    using System;

    public interface IDomainEvent
    {
        Guid EventGuid { get; }
        DateTime OccuredOnUtc { get; }
    }

    public class DomainEvent : IDomainEvent
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