namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using Inventory.Application.Commands;
    using Inventory.Domain.Model.Catalog;
    using Inventory.Domain.Model.Reservations;

    public class ReservationSaga : SagaBase
    {
        private ReservationId _reservationId;

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        private void When(DomainEvent e)
        {
            // Fallback
        }

        private void When(OrderItemAdded e)
        {
            _reservationId = new ReservationId(e.OrderItemId);

            UndispatchedCommands.Add(new ReserveArticle(
                new UserId(e.InitiatorId),
                new OrganizationId(e.OrganizationId),
                new ArticleId(e.ArticleId),
                new OrderId(e.OrderId),
                _reservationId,
                e.Period,
                e.Quantity));
        }

        private void When(OrderItemRemoved e)
        {
            UndispatchedCommands.Add(new CancelReservation(_reservationId));
        }

        private void When(OrderItemQuantityChanged e)
        {
            UndispatchedCommands.Add(new ChangeReservationQuantity(_reservationId));
        }

        private void When(OrderItemPeriodChanged e)
        {
            UndispatchedCommands.Add(new ChangeReservationPeriod(_reservationId));
        }
    }
}