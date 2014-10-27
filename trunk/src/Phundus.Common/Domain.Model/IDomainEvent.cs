namespace Phundus.Common.Domain.Model
{
    using System;

    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccuredOnUtc { get; }
    }
}