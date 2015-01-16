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
    using Stateless;

    public class AllocationSagaManager : SagaManager<AllocationSaga>
    {
        public AllocationSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher) : base(repository, dispatcher)
        {
        }
    }

    public class AllocationSaga : SagaBase
    {
        public enum State
        {
            NotAllocated,
            Allocated,
            Discarded
        }

        private enum Trigger
        {
            ArticleReserved,
            ReservationCancelled
        }

        private readonly StateMachine<State, Trigger> _stateMachine = new StateMachine<State, Trigger>(State.NotAllocated);
        private OrganizationId _organizationId;
        private ArticleId _articleId;
        private StockId _stockId;
        private ReservationId _reservationId;
        private AllocationId _allocationId;
        private Period _period;
        private int _quantity;

        public AllocationSaga()
        {
            _stateMachine.Configure(State.NotAllocated)
                .Permit(Trigger.ArticleReserved, State.Allocated);

            _stateMachine.Configure(State.Allocated)
                .OnEntry(AllocateStock)
                .Permit(Trigger.ReservationCancelled, State.Discarded)
                .Ignore(Trigger.ArticleReserved);

            _stateMachine.Configure(State.Discarded)
                .OnEntry(DiscardAllocation)
                .Ignore(Trigger.ReservationCancelled);
        }

        public State CurrentState { get { return _stateMachine.State; } }

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
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = StockId.Default;
            _reservationId = new ReservationId(e.ReservationId);
            _allocationId = new AllocationId(new Guid(e.ReservationId));
            _period = e.Period;
            _quantity = e.Quantity;

            _stateMachine.Fire(Trigger.ArticleReserved);
        }

        private void When(ReservationCancelled e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = StockId.Default;
            _reservationId = new ReservationId(e.ReservationId);
            _allocationId = new AllocationId(new Guid(e.ReservationId));

            _stateMachine.Fire(Trigger.ReservationCancelled);
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
            Dispatch(new AllocateStock(_organizationId, _articleId, _stockId, _allocationId, _reservationId, _period, _quantity));
        }

        private void DiscardAllocation()
        {
            Dispatch(new DiscardAllocation(_organizationId, _articleId, _stockId, _allocationId));
        }
    }
}