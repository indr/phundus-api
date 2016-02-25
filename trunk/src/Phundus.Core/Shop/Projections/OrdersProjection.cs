namespace Phundus.Shop.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;

    public class OrdersProjection : ReadModelBase<OrdersProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }
    }
}