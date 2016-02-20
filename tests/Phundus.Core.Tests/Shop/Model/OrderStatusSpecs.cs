namespace Phundus.Tests.Shop.Orders.Model
{
    using System;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    public abstract class pending_order_concern : order_concern
    {
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_approved : pending_order_concern
    {
        public Because of = () => sut.Approve(theInitiator);

        public It should_have_status_approved = () => sut.Status.ShouldEqual(OrderStatus.Approved);

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Matches(p => p.ShortOrderId == sut.ShortOrderId.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_rejected : pending_order_concern
    {
        public Because of = () => sut.Reject(theInitiator);

        public It should_have_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Matches(p => p.ShortOrderId == sut.ShortOrderId.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_closed : pending_order_concern
    {
        public Because of = () => sut.Close(theInitiator);

        public It should_have_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Matches(p => p.ShortOrderId == sut.ShortOrderId.Id)));
    }


    public abstract class approved_order_concern : order_concern
    {
        public Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Approve(theInitiator));
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_approved : approved_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Approve(theInitiator));

        public It should_retain_status_approved = () =>
            sut.Status.ShouldEqual(OrderStatus.Approved);

        public It should_throw_order_already_approved = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyApprovedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_closed : approved_order_concern
    {
        public Because of = () => sut.Close(theInitiator);

        public It should_have_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Matches(p => p.ShortOrderId == sut.ShortOrderId.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_rejected : approved_order_concern
    {
        public Because of = () => sut.Reject(theInitiator);

        public It should_have_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Matches(p => p.ShortOrderId == sut.ShortOrderId.Id)));
    }


    public abstract class rejected_order_concern : order_concern
    {
        public Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Reject(theInitiator));
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_approved : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Approve(theInitiator));

        public It should_retain_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_rejected : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Reject(theInitiator));

        public It should_retain_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_closed : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Close(theInitiator));

        public It should_retain_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }


    public abstract class closed_order_concern : order_concern
    {
        public Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Close(theInitiator));
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_approved : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Approve(theInitiator));

        public It should_retain_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_rejected : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Reject(theInitiator));

        public It should_retain_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_closed : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Close(theInitiator));

        public It should_retain_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }
}