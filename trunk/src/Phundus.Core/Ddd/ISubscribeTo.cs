namespace Phundus.Core.Ddd
{
    public interface ISubscribeTo
    {
    }

    public interface ISubscribeTo<in TDomainEvent> : ISubscribeTo
    {
        void Handle(TDomainEvent @event);
    }
}