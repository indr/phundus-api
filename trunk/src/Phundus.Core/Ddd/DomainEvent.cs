namespace Phundus.Core.Ddd
{
    using System;

    public class DomainEvent
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            OccuredOn = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public DateTime OccuredOn { get; private set; }
    }
}