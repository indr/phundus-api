namespace Phundus.Core.Ddd
{
    public interface IEventStore
    {
        void Append(DomainEvent domainEvent);
    }
}