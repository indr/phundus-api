namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Fakes;
    using Machine.Specifications;

    public class saga_manager_concern<TSagaManager> : concern<TSagaManager> where TSagaManager : class
    {
        
    }

    public class reservation_saga_manager_concern : saga_manager_concern<ReservationSagaManager>
    {
        protected static UserId _initiatorId = new UserId(1);
        protected static OrganizationId _organizationId = new OrganizationId(2);
        protected static OrderId orderId = new OrderId(3);
        protected static ArticleId _articleId = new ArticleId(4);
        protected static Period _period = new Period(DateTime.Today, DateTime.Now);
        protected static int _quantity = 1;
        protected static ISagaRepository repository;

        protected static Guid orderItemId = new Guid();

        protected static ReservationSaga saga;

        public Establish ctx = () =>
        {
            saga = new ReservationSaga();
            repository = depends.@on<ISagaRepository>();
            repository.WhenToldTo(x => x.GetById<ReservationSaga>(orderItemId)).Return(saga);
        };
    }

    public class when_reservation_saga_manager_handles_order_item_added : reservation_saga_manager_concern
    {
        public Because of = () =>
        {
            sut.Handle(new OrderItemAdded(_initiatorId, _organizationId, orderId, orderItemId,
                _articleId, _period, _quantity));
        };

        public It should_load_saga_with_order_item_id =
            () => repository.WasToldTo(x => x.GetById<ReservationSaga>(orderItemId));

        public It should_save_uncommitted_events = () => repository.WasToldTo(x => x.Save(saga));
    }

    public class when_reservation_saga_manager_handles_order_item_removed : reservation_saga_manager_concern
    {
        public Because of =
            () => { sut.Handle(new OrderItemRemoved(_initiatorId, _organizationId, orderId, orderItemId)); };

        public It should_load_saga_with_order_item_id =
            () => repository.WasToldTo(x => x.GetById<ReservationSaga>(orderItemId));

        public It should_save_uncommitted_events = () => repository.WasToldTo(x => x.Save(saga));
    }
}