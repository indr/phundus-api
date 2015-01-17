namespace Phundus.Common.EventPublishing
{
    using Domain.Model;

    public interface IEventPublisher
    {
        void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;        
    }
}