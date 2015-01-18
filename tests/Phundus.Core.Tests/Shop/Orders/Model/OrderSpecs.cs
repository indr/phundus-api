namespace Phundus.Core.Tests.Shop.Orders.Model
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Services;
    using Core.Shop.Domain.Model.Identity;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public abstract class order_concern : concern<Order>
    {
        protected static Order order;
        protected static int modifierId = 101;
        protected static UserId _initiatorId = new UserId(101);
        protected static OrganizationId _organizationId = new OrganizationId(1001);
        protected static ArticleId _articleId = new ArticleId(10001);
        protected static Period _period = Period.FromTodayToTomorrow;
        protected static int _quantity = 1;
        protected static Guid _orderItemId;

        private Establish ctx = () =>
        {
            depends.on<Organization>(OrganizationFactory.Create());
            depends.on<Borrower>(BorrowerFactory.Create());
        };
    }

    [Subject(typeof (Order))]
    public class when_an_order_is_created : order_concern
    {
        private static Organization organization;
        private static Borrower borrower;

        public Establish ctx = () =>
        {
            organization = OrganizationFactory.Create();
            borrower = BorrowerFactory.Create();
        };

        public Because of = () =>
        {
            order = new Order(organization, borrower);
        };

        public It should_have_status_pending =
            () => order.Status.ShouldEqual(OrderStatus.Pending);

        public It should_have_the_borrower =
            () => order.Borrower.ShouldEqual(borrower);

        public It should_have_the_created_on_set_to_utc_now =
            () => order.CreatedUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_the_organization =
            () => order.Organization.ShouldEqual(organization);

        public It should_not_have_a_modified_date =
            () => order.ModifiedUtc.ShouldBeNull();

        public It should_not_have_a_modifier =
            () => order.ModifiedBy.ShouldBeNull();
    }

    [Subject(typeof(Order))]
    public class when_period_of_an_order_item_is_changed : order_concern
    {
        private Establish ctx = () =>
        {
            var article = new Article(_articleId, _organizationId.Id, "Article");
            order = OrderFactory.CreatePending();
            _orderItemId = order.AddItem(_initiatorId, article, _period.FromUtc, _period.ToUtc, _quantity).Id;
        };

        private Because of = () => order.ChangeItemPeriod(_initiatorId, _orderItemId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public It should_publish_order_item_period_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemPeriodChanged>.Is.NotNull));
    }

    [Subject(typeof(Order))]
    public class when_period_of_an_order_item_is_changed_to_the_same_period : order_concern
    {
        private Establish ctx = () =>
        {
            var article = new Article(_articleId, _organizationId.Id, "Article");
            order = OrderFactory.CreatePending();
            _orderItemId = order.AddItem(_initiatorId, article, _period.FromUtc, _period.ToUtc, _quantity).Id;
        };

        private Because of = () => order.ChangeItemPeriod(_initiatorId, _orderItemId, _period.FromUtc, _period.ToUtc);

        public It should_not_publish_order_item_period_changed =
            () => publisher.WasNotToldTo(x => x.Publish(Arg<OrderItemPeriodChanged>.Is.NotNull));
    }

    [Subject(typeof (Order))]
    public class when_quantity_of_an_order_item_is_changed : order_concern
    {
        private Establish ctx = () =>
        {
            var article = new Article(_articleId, _organizationId.Id, "Article");
            order = OrderFactory.CreatePending();
            _orderItemId = order.AddItem(_initiatorId, article, _period.FromUtc, _period.ToUtc, _quantity).Id;
        };

        private Because of = () => order.ChangeQuantity(_initiatorId, _orderItemId, _quantity + 1);

        public It should_publish_order_item_quantity_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemQuantityChanged>.Is.NotNull));
    }

    [Subject(typeof(Order))]
    public class when_quantity_of_an_order_item_is_changed_to_the_same_quantity : order_concern
    {
        private Establish ctx = () =>
        {
            var article = new Article(_articleId, _organizationId.Id, "Article");
            order = OrderFactory.CreatePending();
            _orderItemId = order.AddItem(_initiatorId, article, _period.FromUtc, _period.ToUtc, _quantity).Id;
        };

        private Because of = () => order.ChangeQuantity(_initiatorId, _orderItemId, _quantity);

        public It should_not_publish_order_item_quantity_changed =
            () => publisher.WasNotToldTo(x => x.Publish(Arg<OrderItemQuantityChanged>.Is.NotNull));
    }

    public abstract class pending_order_concern : order_concern
    {
        public Establish ctx = () => { order = OrderFactory.CreatePending(); };
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_approved : pending_order_concern
    {
        public Because of = () => order.Approve(modifierId);

        public It should_have_status_approved = () => order.Status.ShouldEqual(OrderStatus.Approved);

        public It should_be_modified_on =
            () => order.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Matches(p => p.OrderId == order.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_rejected : pending_order_concern
    {
        public Because of = () => order.Reject(modifierId);

        public It should_have_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_be_modified_on =
            () => order.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Matches(p => p.OrderId == order.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_closed : pending_order_concern
    {
        public Because of = () => order.Close(modifierId);

        public It should_have_status_closed = () => order.Status.ShouldEqual(OrderStatus.Closed);

        public It should_be_modified_on =
            () => order.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Matches(p => p.OrderId == order.Id)));
    }

    public abstract class approved_order_concern : order_concern
    {
        public Establish ctx = () => { order = OrderFactory.CreateApproved(); };
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_approved : approved_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Approve(modifierId));

        public It should_retain_status_approved = () => order.Status.ShouldEqual(OrderStatus.Approved);

        public It should_throw_order_already_approved =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyApprovedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_closed : approved_order_concern
    {
        public Because of = () => order.Close(modifierId);

        public It should_have_status_closed = () => order.Status.ShouldEqual(OrderStatus.Closed);

        public It should_be_modified_on =
            () => order.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Matches(p => p.OrderId == order.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_rejected : approved_order_concern
    {
        public Because of = () => order.Reject(modifierId);

        public It should_have_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_be_modified_on =
            () => order.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    
        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Matches(p => p.OrderId == order.Id)));
    }


    public abstract class rejected_order_concern : order_concern
    {
        public Establish ctx = () => { order = OrderFactory.CreateRejected(); };
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_approved : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Approve(modifierId));

        public It should_retain_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_rejected : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Reject(modifierId));

        public It should_retain_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_closed : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Close(modifierId));

        public It should_retain_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }


    public abstract class closed_order_concern : order_concern
    {
        public Establish ctx = () => { order = OrderFactory.CreateClosed(); };
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_approved : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Approve(modifierId));

        public It should_retain_status_closed = () => order.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_rejected : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Reject(modifierId));

        public It should_retain_status_closed = () => order.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_closed : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => order.Close(modifierId));

        public It should_retain_status_closed = () => order.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }


    public class OrderFactory
    {
        public static Order CreatePending()
        {
            return new Order(OrganizationFactory.Create(), BorrowerFactory.Create());
        }

        public static Order CreateClosed()
        {
            var result = CreatePending();
            result.Close(1);
            return result;
        }

        public static Order CreateRejected()
        {
            var result = CreatePending();
            result.Reject(1);
            return result;
        }

        public static Order CreateApproved()
        {
            var result = CreatePending();
            result.Approve(1);
            return result;
        }
    }
}