namespace Phundus.Shop.Projections
{
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;

    public class OrdersProjection : ReadModelBase<OrdersProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic)domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }
    }
}