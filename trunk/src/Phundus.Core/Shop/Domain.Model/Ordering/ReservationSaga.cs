namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using Inventory.Application.Commands;
    using Inventory.Domain.Model.Catalog;
    using Inventory.Domain.Model.Reservations;
    using Orders.Model;

    public class ReservationSaga : StateMachineSagaBase<ReservationSaga.State, ReservationSaga.Trigger>
    {
        public enum State
        {
            New,
            Reserved,
            Cancelled
        }

        public enum Trigger
        {
            OrderItemAdded,
            OrderItemRemoved,
            OrderRejected,
            OrderClosed
        }

        private ArticleId _articleId;
        private UserId _initiatorId;
        private OrderId _orderId;
        private OrganizationId _organizationId;
        private Period _period;
        private int _quantity;
        private ReservationId _reservationId;

        public ReservationSaga() : base(State.New)
        {
            StateMachine.Configure(State.New)
                .Permit(Trigger.OrderItemAdded, State.Reserved);

            StateMachine.Configure(State.Reserved)
                .OnEntry(ReserveArticle)
                .Permit(Trigger.OrderItemRemoved, State.Cancelled)
                .Permit(Trigger.OrderClosed, State.Cancelled)
                .Permit(Trigger.OrderRejected, State.Cancelled)
                .Ignore(Trigger.OrderItemAdded);

            StateMachine.Configure(State.Cancelled)
                .OnEntry(CancelReservation)
                .Ignore(Trigger.OrderItemRemoved)
                .Ignore(Trigger.OrderClosed)
                .Ignore(Trigger.OrderRejected);
        }

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
            _organizationId = new OrganizationId(e.OrganizationId);
            _initiatorId = new UserId(e.InitiatorId);
            _articleId = new ArticleId(e.ArticleId);
            _orderId = new OrderId(e.OrderId);
            _reservationId = new ReservationId(e.OrderItemId);
            _period = e.Period;
            _quantity = e.Quantity;

            StateMachine.Fire(Trigger.OrderItemAdded);
        }

        private void When(OrderItemRemoved e)
        {
            AssertionConcern.AssertArgumentEquals(_organizationId.Id, e.OrganizationId,
                "Organization id must be the same.");
            AssertionConcern.AssertArgumentEquals(_articleId.Id, e.ArticleId, "Article id must be the same.");
            AssertionConcern.AssertArgumentEquals(_orderId.Id, e.OrderId, "Order id must be the same.");
            AssertionConcern.AssertArgumentEquals(_reservationId.Id, e.OrderItemId.ToString(),
                "Reservation id must be equal to order item id.");

            _initiatorId = new UserId(e.InitiatorId);

            StateMachine.Fire(Trigger.OrderItemRemoved);
        }

        private void When(OrderItemQuantityChanged e)
        {
            AssertionConcern.AssertArgumentEquals(_organizationId.Id, e.OrganizationId,
                "Organization id must be the same.");
            AssertionConcern.AssertArgumentEquals(_articleId.Id, e.ArticleId, "Article id must be the same.");
            AssertionConcern.AssertArgumentEquals(_orderId.Id, e.OrderId, "Order id must be the same.");
            AssertionConcern.AssertArgumentEquals(_reservationId.Id, e.OrderItemId.ToString(),
                "Reservation id must be equal to order item id.");

            _initiatorId = new UserId(e.InitiatorId);
            _quantity = e.NewQuantity;

            Dispatch(new ChangeReservationQuantity(_organizationId, _reservationId, _quantity));
        }

        private void When(OrderItemPeriodChanged e)
        {
            AssertionConcern.AssertArgumentEquals(_organizationId.Id, e.OrganizationId,
                "Organization id must be the same.");
            AssertionConcern.AssertArgumentEquals(_articleId.Id, e.ArticleId, "Article id must be the same.");
            AssertionConcern.AssertArgumentEquals(_orderId.Id, e.OrderId, "Order id must be the same.");
            AssertionConcern.AssertArgumentEquals(_reservationId.Id, e.OrderItemId.ToString(),
                "Reservation id must be equal to order item id.");

            _initiatorId = new UserId(e.InitiatorId);
            _period = new Period(e.NewFromUtc, e.NewToUtc);

            Dispatch(new ChangeReservationPeriod(_organizationId, _reservationId, _period));
        }

        private void When(OrderRejected e)
        {
            AssertionConcern.AssertArgumentEquals(_organizationId.Id, e.OrganizationId,
                "Organization id must be the same.");
            AssertionConcern.AssertArgumentEquals(_orderId.Id, e.OrderId, "Order id must be the same.");

            StateMachine.Fire(Trigger.OrderRejected);
        }

        private void When(OrderClosed e)
        {
            AssertionConcern.AssertArgumentEquals(_organizationId.Id, e.OrganizationId,
                "Organization id must be the same.");
            AssertionConcern.AssertArgumentEquals(_orderId.Id, e.OrderId, "Order id must be the same.");

            StateMachine.Fire(Trigger.OrderClosed);
        }

        private void ReserveArticle()
        {
            Dispatch(new ReserveArticle(
                _initiatorId,
                _organizationId,
                _articleId,
                _orderId,
                _reservationId,
                _period,
                _quantity));
        }

        private void CancelReservation()
        {
            Dispatch(new CancelReservation(
                _initiatorId,
                _organizationId,
                _reservationId));
        }
    }
}