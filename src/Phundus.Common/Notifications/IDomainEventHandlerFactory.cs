namespace Phundus.Common.Notifications
{
    public interface IDomainEventHandlerFactory
    {
        IStoredEventsConsumer[] GetDomainEventHandlers();
    }
}