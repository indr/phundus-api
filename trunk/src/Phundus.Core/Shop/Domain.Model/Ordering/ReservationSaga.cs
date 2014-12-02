namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using Inventory.Application.Commands;
    using Inventory.Domain.Model.Catalog;

    public class ReservationSaga : SagaBase
    {
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
            UndispatchedCommands.Add(new ReserveArticle(new UserId(e.InitiatorId), new OrganizationId(e.OrganizationId),
                new ArticleId(e.ArticleId), new OrderId(e.OrderId), new CorrelationId(e.OrderItemId), e.FromUtc, e.ToUtc,
                e.Quantity));
        }

        private void When(OrderItemRemoved e)
        {
            UndispatchedCommands.Add(new CancelReservation());
        }
    }
}