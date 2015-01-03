namespace Phundus.Core.Specs.Shop.Ordering
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    public class SagaConcern<TSaga> where TSaga : ISaga, new()
    {
        protected SagaConcern(PastEvents pastEvents)
        {
            PastEvents = pastEvents;

            Saga = new TSaga();
        }

        protected TSaga Saga { get; set; }

        protected PastEvents PastEvents { get; set; }

        protected void Transition(IDomainEvent evnt)
        {
            foreach (var each in PastEvents.Events)
                Saga.Transition(each);

            Saga.ClearUncommittedEvents();
            Saga.ClearUndispatchedCommands();

            Saga.Transition(evnt);
        }

        protected void AssertUndispatchedCommand<TCommand>(TCommand command)
        {
            Assert.That(Saga.UndispatchedCommands.Count, Is.EqualTo(1));
            var actual = Saga.UndispatchedCommands.First();
            Assert.That(actual, Is.InstanceOf<TCommand>());
        }

        protected void AssertNoUndispatchedCommands()
        {
            Assert.That(Saga.UndispatchedCommands.Count, Is.EqualTo(0));
        }
    }

    [Binding]
    public class ReservationSagaSteps : SagaConcern<ReservationSaga>
    {
        private readonly ArticleId _articleId = new ArticleId(1);
        private readonly UserId _initiatorId = new UserId(1);
        private readonly OrderId _orderId = new OrderId(1);
        private readonly Guid _orderItemId = new Guid();
        private readonly OrganizationId _organizationId = new OrganizationId(1);
        private readonly Period _period = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        private readonly int _quantity = 1;


        public ReservationSagaSteps(PastEvents pastEvents) : base(pastEvents)
        {
            PastEvents = pastEvents;
        }

        [Given(@"empty order created")]
        public void GivenEmptyOrderCreated()
        {
            PastEvents.Add(new OrderCreated(_orderId));
        }

        [When(@"order item added")]
        public void WhenOrderItemAdded()
        {
            var evnt = new OrderItemAdded(_initiatorId, _organizationId, _orderId, _orderItemId, _articleId, _period,
                _quantity);

            Transition(evnt);
        }

        [Then(@"reserve article")]
        public void ThenReserveArticle()
        {
            AssertUndispatchedCommand<ReserveArticle>(null);
        }

        [Given(@"order item added")]
        public void GivenOrderItemAdded()
        {
            PastEvents.Add(new OrderItemAdded(_initiatorId, _organizationId, _orderId, _orderItemId, _articleId, _period,
                _quantity));
        }

        [Then(@"no commands dispatched")]
        public void ThenNoCommandsDispatched()
        {
            AssertNoUndispatchedCommands();
        }

        [When(@"order item removed")]
        public void WhenOrderItemRemoved()
        {
            Transition(new OrderItemRemoved(_initiatorId, _organizationId, _orderId, _orderItemId));
        }

        [Then(@"cancel reservation")]
        public void ThenCancelReservation()
        {
            AssertUndispatchedCommand<CancelReservation>(null);
        }

        [When(@"order item period changed")]
        public void WhenOrderItemPeriodChanged()
        {
            Transition(new OrderItemPeriodChanged(_initiatorId, _organizationId, _orderId, _orderItemId, _period,
                new Period(DateTime.Today, DateTime.Today.AddDays(2))));
        }

        [Then(@"change reservation period")]
        public void ThenChangeReservationPeriod()
        {
            AssertUndispatchedCommand<ChangeReservationPeriod>(null);
        }

        [When(@"order item quantity changed")]
        public void WhenOrderItemQuantityChanged()
        {
            Transition(new OrderItemQuantityChanged(_initiatorId, _organizationId, _orderId, _orderItemId, _quantity, 2));
        }

        [Then(@"change reservation quantity")]
        public void ThenChangeReservationQuantity()
        {
            AssertUndispatchedCommand(new ChangeReservationQuantity(new ReservationId(Guid.Empty)));
        }
    }
}