namespace Phundus.Common.Domain.Model
{
    using System;

    public class DomainEvent
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            OccuredOnUtc = DateTime.UtcNow;
            Version = -1;
        }

        public Guid Id { get; private set; }

        public DateTime OccuredOnUtc { get; private set; }

        // TODO: DomainEvent.Version
        public int Version { get; private set; }
    }
}