namespace Phundus.Common.Notifications
{
    public interface IDomainEventHandlerFactory
    {
        IDomainEventHandler[] GetDomainEventHandlers();
    }
}