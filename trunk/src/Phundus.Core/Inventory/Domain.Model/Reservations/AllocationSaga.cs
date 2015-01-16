namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using Application.Commands;
    using Catalog;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Management;

    public class AllocationSagaManager : SagaManager<AllocationSaga>
    {
        public AllocationSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher) : base(repository, dispatcher)
        {
        }
    }

    public class AllocationSaga : SagaBase
    {
        protected override void When(IDomainEvent e)
        {
            When((dynamic)e);
        }

        private void When(DomainEvent e)
        {
            // Fallback
        }

        private void When(ArticleReserved e)
        {
            Dispatch(new AllocateStock(
                new OrganizationId(e.OrganizationId),
                new ArticleId(e.ArticleId), 
                StockId.Default, 
                new AllocationId(new Guid(e.ReservationId)), 
                new ReservationId(e.ReservationId), 
                e.Period, e.Quantity));
        }

        private void When(ReservationCancelled e)
        {
            Dispatch(new DiscardAllocation(
                new OrganizationId(e.OrganizationId), 
                new ArticleId(e.ArticleId),
                StockId.Default,
                new AllocationId(new Guid(e.ReservationId))
                ));
        }

        private void When(ReservationPeriodChanged e)
        {
            Dispatch(new ChangeAllocationPeriod(
                new OrganizationId(e.OrganizationId),
                new ArticleId(e.ArticleId),
                StockId.Default,
                new AllocationId(new Guid(e.ReservationId)),
                e.NewPeriod
                ));
        }

        private void When(ReservationQuantityChanged e)
        {
            Dispatch(new ChangeAllocationQuantity(
                new OrganizationId(e.OrganizationId),
                new ArticleId(e.ArticleId),
                StockId.Default,
                new AllocationId(new Guid(e.ReservationId)),
                e.NewQuantity
                ));
        }
    }
}