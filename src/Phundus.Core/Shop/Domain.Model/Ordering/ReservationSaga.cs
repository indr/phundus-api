namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using System;
    using Common.Domain.Model;
    using Inventory.Application.Commands;
    using Orders.Model;

    public class ReservationSaga : SagaBase
    {
        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        private void When(OrderItemAdded e)
        {
            UndispatchedCommands.Add(new ReserveArticle(0, 0, 0, new OrderId(e.OrderId),
                new CorrelationId(e.OrderItemId), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
        }
    }
}