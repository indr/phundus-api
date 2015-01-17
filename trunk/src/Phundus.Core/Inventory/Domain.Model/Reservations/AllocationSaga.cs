namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using Application.Commands;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Management;

    public class AllocationSaga : StateMachineSagaBase<AllocationSaga.State, AllocationSaga.Trigger>
    {
        public enum State
        {
            NotAllocated,
            Allocated,
            Discarded
        }

        public enum Trigger
        {
            ArticleReserved,
            ReservationCancelled
        }

        private AllocationId _allocationId;
        private ArticleId _articleId;
        private OrganizationId _organizationId;
        private Period _period;
        private int _quantity;
        private ReservationId _reservationId;
        private StockId _stockId;

        public AllocationSaga() : base(State.NotAllocated)
        {
            StateMachine.Configure(State.NotAllocated)
                .Permit(Trigger.ArticleReserved, State.Allocated);

            StateMachine.Configure(State.Allocated)
                .OnEntry(AllocateStock)
                .Permit(Trigger.ReservationCancelled, State.Discarded)
                .Ignore(Trigger.ArticleReserved);

            StateMachine.Configure(State.Discarded)
                .OnEntry(DiscardAllocation)
                .Ignore(Trigger.ReservationCancelled);
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic) e);
        }

        private void When(DomainEvent e)
        {
            // Fallback
        }

        private void When(ArticleReserved e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = StockId.Default;
            _reservationId = new ReservationId(e.ReservationId);
            _allocationId = new AllocationId(new Guid(e.ReservationId));
            _period = e.Period;
            _quantity = e.Quantity;

            StateMachine.Fire(Trigger.ArticleReserved);
        }

        private void When(ReservationCancelled e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = StockId.Default;
            _reservationId = new ReservationId(e.ReservationId);
            _allocationId = new AllocationId(new Guid(e.ReservationId));

            StateMachine.Fire(Trigger.ReservationCancelled);
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

        private void AllocateStock()
        {
            Dispatch(new AllocateStock(_organizationId, _articleId, _stockId, _allocationId, _reservationId, _period,
                _quantity));
        }

        private void DiscardAllocation()
        {
            Dispatch(new DiscardAllocation(_organizationId, _articleId, _stockId, _allocationId));
        }
    }
}