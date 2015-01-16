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

        protected AllocationSagaSteps(PastEvents pastEvents) : base(pastEvents)
        {
        }

        [When(@"article reserved")]
        public void WhenIReserveArticle()
        {
            Transition(new ArticleReserved(_organizationId, _articleId, _reservationId, _orderId, _period, _quantity));
        }

        [Then(@"allocate stock")]
        public void ThenAllocateStock()
        {
            AssertUndispatchedCommand<AllocateStock>();
        }
    }
}