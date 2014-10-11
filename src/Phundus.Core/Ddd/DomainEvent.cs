namespace Phundus.Core.Ddd
{
    using System;

    public class DomainEvent
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            OccuredOnUtc = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public DateTime OccuredOnUtc { get; private set; }
    }
}