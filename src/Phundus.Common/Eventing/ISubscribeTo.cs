namespace Phundus.Common.Eventing
{
    public interface ISubscribeTo
    {
    }

    public interface ISubscribeTo<in TDomainEvent> : ISubscribeTo
    {
        void Handle(TDomainEvent e);
    }
}