namespace Phundus.Common.EventPublishing
{
    using Domain.Model;

    public interface IEventPublisher
    {
        void Publish<TDomainEvent>(TDomainEvent @event, bool store = true) where TDomainEvent : DomainEvent;
    }
}