﻿namespace Phundus.Tests.Shop.Model
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;    
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public abstract class pending_order_concern : order_concern
    {
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_approved : pending_order_concern
    {
        public Because of = () =>
            sut.Approve(theManager);

        private It should_have_mutating_event_order_approved = () =>
            mutatingEvent<OrderApproved>(p =>
                p.OrderId == theOrderId.Id);

        public It should_have_status_approved = () =>
            sut.Status.ShouldEqual(OrderStatus.Approved);
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_rejected : pending_order_concern
    {
        public Because of = () =>
            sut.Reject(theManager);

        private It should_have_mutating_event_order_rejected = () =>
            mutatingEvent<OrderRejected>(p =>
                p.OrderId == theOrderId.Id);

        public It should_have_status_rejected = () =>
            sut.Status.ShouldEqual(OrderStatus.Rejected);
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_closed : pending_order_concern
    {
        public Because of = () =>
            sut.Close(theManager);

        private It should_have_mutating_event_order_closed = () =>
            mutatingEvent<OrderClosed>(p =>
                p.OrderId == theOrderId.Id);

        public It should_have_status_closed = () =>
            sut.Status.ShouldEqual(OrderStatus.Closed);
    }


    public abstract class approved_order_concern : order_concern
    {
        public Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Approve(theManager));
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_approved : approved_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Approve(theManager));

        public It should_retain_status_approved = () =>
            sut.Status.ShouldEqual(OrderStatus.Approved);

        public It should_throw_order_already_approved = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyApprovedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_closed : approved_order_concern
    {
        public Because of = () =>
            sut.Close(theManager);

        public It should_have_mutating_event_order_closed = () =>
            mutatingEvent<OrderClosed>(p =>
                p.OrderId == theOrderId.Id);

        public It should_have_status_closed = () =>
            sut.Status.ShouldEqual(OrderStatus.Closed);
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_rejected : approved_order_concern
    {
        public Because of = () =>
            sut.Reject(theManager);

        private It should_have_mutating_event_order_rejected = () =>
            mutatingEvent<OrderRejected>(p =>
                p.OrderId == theOrderId.Id);

        public It should_have_status_rejected = () =>
            sut.Status.ShouldEqual(OrderStatus.Rejected);
    }

    public abstract class rejected_order_concern : order_concern
    {
        public Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Reject(theManager));
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_approved : rejected_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Approve(theManager));

        public It should_retain_status_rejected = () =>
            sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_rejected : rejected_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Reject(theManager));

        public It should_retain_status_rejected = () =>
            sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_closed : rejected_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Close(theManager));

        public It should_retain_status_rejected = () =>
            sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }


    public abstract class closed_order_concern : order_concern
    {
        public Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Close(theManager));
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_approved : closed_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Approve(theManager));

        public It should_retain_status_closed = () =>
            sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_rejected : closed_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Reject(theManager));

        public It should_retain_status_closed = () =>
            sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_closed : closed_order_concern
    {
        public Because of = () =>
            spec.catch_exception(() =>
                sut.Close(theManager));

        public It should_retain_status_closed = () =>
            sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed = () =>
            spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }
}