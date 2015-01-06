namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using Inventory.Application.Commands;
    using Inventory.Domain.Model.Catalog;
    using Inventory.Domain.Model.Reservations;
    using Orders.Model;

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
            // TODO: Use Statless state machine
            if (_reservationId != null)
                return;

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
            UndispatchedCommands.Add(new CancelReservation(new OrganizationId(e.OrganizationId), new ArticleId(e.ArticleId),  _reservationId));
        }

        private void When(OrderItemQuantityChanged e)
        {
            UndispatchedCommands.Add(new ChangeReservationQuantity(new OrganizationId(e.OrganizationId), new ArticleId(e.ArticleId), _reservationId, e.NewQuantity));
        }

        private void When(OrderItemPeriodChanged e)
        {
            UndispatchedCommands.Add(new ChangeReservationPeriod(new OrganizationId(e.OrganizationId), new ArticleId(e.ArticleId), _reservationId, new Period(e.NewFromUtc, e.NewToUtc)));
        }

        private void When(OrderRejected e)
        {
            //UndispatchedCommands.Add(new CancelReservation(new OrganizationId(e.OrganizationId), new ArticleId(each),  _reservationId));
        }

        private void When(OrderClosed e)
        {
            //UndispatchedCommands.Add(new CancelReservation(_reservationId));
        }
    }
}