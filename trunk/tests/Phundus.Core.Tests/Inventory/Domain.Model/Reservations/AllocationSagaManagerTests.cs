namespace Phundus.Core.Tests.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Linq;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Core.Cqrs;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof(AllocationSagaManager))]
    public class AllocationSagaManagerTests : concern<AllocationSagaManager>
    {
        protected static IDomainEvent _event;

        protected static OrganizationId _organizationId = new OrganizationId(1001);
        protected static ArticleId _articleId = new ArticleId(10001);
        protected static ReservationId _reservationId = new ReservationId();
        protected static OrderId _orderId = new OrderId(1);
        protected static Period _period = Period.FromTodayToTomorrow;
        protected static int _quantity = 1;

        private Establish ctx = () =>
        {
            _saga = mock.stub<AllocationSaga>();
            _sagaRepository = depends.on<ISagaRepository>();
            _sagaRepository.WhenToldTo(x => x.GetById<AllocationSaga>(new Guid(_reservationId.Id))).Return(_saga);
            _commandDispatcher = depends.on<ICommandDispatcher>();
        };

        private Because of = () => sut.Handle((dynamic)_event);

        protected static ISagaRepository _sagaRepository;
        private static ICommandDispatcher _commandDispatcher;
        protected static AllocationSaga _saga;

        protected static void AssertRepositoryGetById(string correlationId)
        {
            _sagaRepository.WasToldTo(x => x.GetById<AllocationSaga>(new Guid(correlationId)));
        }

        protected static void AssertTransition()
        {
            _saga.WasToldTo(x => x.Transition(_event));
        }
    }

    public class when_article_reserved_is_handled : AllocationSagaManagerTests
    {
        private Establish ctx = () =>
        {
            _event = new ArticleReserved(_organizationId, _articleId, _reservationId, _orderId, _period, _quantity);
        };

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_reservation_cancelled_is_handled : AllocationSagaManagerTests
    {
        private Establish ctx =
            () => _event = new ReservationCancelled(_organizationId, _articleId, _reservationId, _period, _quantity);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);
        
        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_reservation_period_changed_is_handled : AllocationSagaManagerTests
    {
        private Establish ctx =
            () => _event = new ReservationPeriodChanged(_organizationId, _articleId, _reservationId, _period, _period);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_reservation_quantity_changed_is_handled : AllocationSagaManagerTests
    {
        private Establish ctx =
            () => _event = new ReservationQuantityChanged(_organizationId, _articleId, _reservationId, _quantity, _quantity);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }
}