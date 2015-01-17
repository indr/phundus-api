namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using Application.Commands;
    using Catalog;
    using Common.Domain.Model;
    using Common.EventPublishing;
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Management;
    using Stateless;

    public class AllocationSagaManager : SagaManager<AllocationSaga>, ISubscribeTo<ArticleReserved>,
        ISubscribeTo<ReservationCancelled>, ISubscribeTo<ReservationPeriodChanged>,
        ISubscribeTo<ReservationQuantityChanged>
    {
        public AllocationSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher)
            : base(repository, dispatcher)
        {
        }

        public void Handle(ArticleReserved @event)
        {
            Transition(@event.ReservationId, @event);
        }

        public void Handle(ReservationCancelled @event)
        {
            Transition(@event.ReservationId, @event);
        }

        public void Handle(ReservationPeriodChanged @event)
        {
            Transition(@event.ReservationId, @event);
        }

        public void Handle(ReservationQuantityChanged @event)
        {
            Transition(@event.ReservationId, @event);
        }
    }

    public class AllocationSaga : SagaBase
    {
        private readonly StateMachine<State, Trigger> _stateMachine =
            new StateMachine<State, Trigger>(State.NotAllocated);

        private AllocationId _allocationId;
        private ArticleId _articleId;
        private OrganizationId _organizationId;
        private Period _period;
        private int _quantity;
        private ReservationId _reservationId;
        private StockId _stockId;

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
            Dispatch(new AllocateStock(_organizationId, _articleId, _stockId, _allocationId, _reservationId, _period,
                _quantity));
        }

        private void DiscardAllocation()
        {
            Dispatch(new DiscardAllocation(_organizationId, _articleId, _stockId, _allocationId));
        }

        private enum State
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
    }
}