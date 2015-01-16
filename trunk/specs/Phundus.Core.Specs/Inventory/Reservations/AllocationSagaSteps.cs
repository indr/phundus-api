namespace Phundus.Core.Specs.Inventory.Reservations
{
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class AllocationSagaSteps : SagaConcern<AllocationSaga>
    {
        private UserId _initiatorId = new UserId(1);
        private OrganizationId _organizationId = new OrganizationId(1001);
        private ArticleId _articleId = new ArticleId(10001);
        private OrderId _orderId = new OrderId(101);
        private ReservationId _reservationId = new ReservationId();
        private Period _period = Period.FromTodayToTomorrow;
        private int _quantity = 1;

        protected AllocationSagaSteps(SagaContext context, PastEvents pastEvents) : base(context, pastEvents)
        {
        }

        [When(@"article reserved")]
        public void WhenIReserveArticle()
        {
            Transition(CreateArticleReserved());
        }

        private ArticleReserved CreateArticleReserved()
        {
            return new ArticleReserved(_organizationId, _articleId, _reservationId, _orderId, _period, _quantity);
        }

        [Then(@"allocate stock")]
        public void ThenAllocateStock()
        {
            AssertUndispatchedCommand<AllocateStock>();
        }

        [Given(@"article reserved")]
        public void GivenArticleReserved()
        {
            PastEvents.Add(CreateArticleReserved());
        }
        
        [Given(@"reservation cancelled")]
        public void GivenReservationCancelled()
        {
            PastEvents.Add(CreateReservationCancelled());
        }

        [When(@"reservation cancelled")]
        public void WhenReservationCancelled()
        {
            Transition(CreateReservationCancelled());
        }

        private IDomainEvent CreateReservationCancelled()
        {
            return new ReservationCancelled(_organizationId, _articleId, _reservationId, _period, _quantity);
        }

        [Then(@"discard allocation")]
        public void ThenDiscardAllocation()
        {
            AssertUndispatchedCommand<DiscardAllocation>();
        }

        [When(@"reservation period changed")]
        public void WhenReservationPeriodChanged()
        {
            Transition(CreateReservationPeriodChanged());
        }

        private IDomainEvent CreateReservationPeriodChanged()
        {
            return new ReservationPeriodChanged(_organizationId, _articleId, _reservationId, _period, _period);
        }

        [Then(@"change allocation period")]
        public void ThenChangeAllocationPeriod()
        {
            AssertUndispatchedCommand<ChangeAllocationPeriod>();
        }

        [When(@"reservation quantity changed")]
        public void WhenReservationQuantityChanged()
        {
            Transition(CreateReservationQuantityChanged());
        }
        

        private IDomainEvent CreateReservationQuantityChanged()
        {
            return new ReservationQuantityChanged(_organizationId, _articleId, _reservationId, _quantity, _quantity);
        }

        [Then(@"change allocation quantity")]
        public void ThenChangeAllocationQuantity()
        {
            AssertUndispatchedCommand<ChangeAllocationQuantity>();
        }

        [Then(@"allocation saga state is Allocated")]
        public void ThenAllocationSagaStateIsHasAllocation()
        {
            Assert.That(Saga.CurrentState, Is.EqualTo(AllocationSaga.State.Allocated));
        }
    }
}