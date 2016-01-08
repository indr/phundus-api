namespace Phundus.Infrastructure
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        DateTime Today { get; }
    }
}