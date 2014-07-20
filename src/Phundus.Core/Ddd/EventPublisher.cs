namespace Phundus.Core.Ddd
{
    using Castle.Windsor;

    public static class EventPublisher
    {
        public static IWindsorContainer Container { get; set; }

        public static void Publish(DomainEvent @event)
        {
            
        }
    }

    public class DomainEvent
    {
    }

    public interface ISubscribeTo
    {
    }

    public interface ISubscribeTo<in TDomainEvent> : ISubscribeTo where TDomainEvent : DomainEvent
    {
        void Handle(TDomainEvent @event);
    }
}