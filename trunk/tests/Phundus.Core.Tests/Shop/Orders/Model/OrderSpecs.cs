namespace Phundus.Core.Tests.Shop.Orders.Model
{
    using System;
    using Core.Shop.Contracts.Model;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public abstract class order_concern : concern<Order>
    {
        protected static Order order;
        protected static int modifierId = 101;
    }

    [Subject(typeof (Order))]
    public class when_an_order_is_created : order_concern
    {
        private const int organizationId = 1;
        private static Borrower borrower;

        public Establish ctx = () => { borrower = BorrowerFactory.Create(); };

        public Because of = () => { order = new Order(organizationId, borrower); };

        public It should_have_status_pending =
            () => order.Status.ShouldEqual(OrderStatus.Pending);

        public It should_have_the_borrower =
            () => order.Borrower.ShouldEqual(borrower);

        public It should_have_the_created_on_set_to_utc_now =
            () => order.CreatedOn.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_the_organization_id =
            () => order.OrganizationId.ShouldEqual(organizationId);

        public It should_not_have_a_modified_date =
            () => order.ModifiedOn.ShouldBeNull();

        public It should_not_have_a_modifier =
            () => order.ModifiedBy.ShouldBeNull();
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
            () => order.ModifiedOn.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Is.NotNull));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_rejected : pending_order_concern
    {
        public Because of = () => order.Reject(modifierId);

        public It should_have_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_be_modified_on =
            () => order.ModifiedOn.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Is.NotNull));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_closed : pending_order_concern
    {
        public Because of = () => order.Close(modifierId);

        public It should_have_status_closed = () => order.Status.ShouldEqual(OrderStatus.Closed);

        public It should_be_modified_on =
            () => order.ModifiedOn.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Is.NotNull));
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
            () => order.ModifiedOn.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Is.NotNull));
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_rejected : approved_order_concern
    {
        public Because of = () => order.Reject(modifierId);

        public It should_have_status_rejected = () => order.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_be_modified_on =
            () => order.ModifiedOn.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_be_modified_by =
            () => order.ModifiedBy.Value.ShouldEqual(modifierId);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Is.NotNull));
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
            return new Order(1, BorrowerFactory.Create());
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