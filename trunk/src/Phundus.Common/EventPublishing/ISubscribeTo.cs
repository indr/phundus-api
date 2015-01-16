namespace Phundus.Common.EventPublishing
{
    public interface ISubscribeTo
    {
    }

    public interface ISubscribeTo<in TDomainEvent> : ISubscribeTo
    {
        void Handle(TDomainEvent @event);
    }
}