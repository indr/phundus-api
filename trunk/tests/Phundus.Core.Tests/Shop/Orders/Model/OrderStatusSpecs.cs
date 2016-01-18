namespace Phundus.Tests.Shop.Orders.Model
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Contracts.Model;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    public abstract class pending_order_concern : order_concern
    {
        public Establish ctx = () => { sut = OrderFactory.CreatePending(); };
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_approved : pending_order_concern
    {
        public Because of = () => sut.Approve(theInitiatorId);

        public It should_be_modified_by =
            () => sut.ModifiedBy.ShouldEqual(theInitiatorId);

        public It should_be_modified_on =
            () => sut.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_status_approved = () => sut.Status.ShouldEqual(OrderStatus.Approved);

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Matches(p => p.OrderId == sut.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_rejected : pending_order_concern
    {
        public Because of = () => sut.Reject(theInitiatorId);

        public It should_be_modified_by =
            () => sut.ModifiedBy.ShouldEqual(theInitiatorId);

        public It should_be_modified_on =
            () => sut.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Matches(p => p.OrderId == sut.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_pending_order_is_closed : pending_order_concern
    {
        public Because of = () => sut.Close(theInitiatorId);

        public It should_be_modified_by =
            () => sut.ModifiedBy.ShouldEqual(theInitiatorId);

        public It should_be_modified_on =
            () => sut.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Matches(p => p.OrderId == sut.Id)));
    }


    public abstract class approved_order_concern : order_concern
    {
        public Establish ctx = () => { sut = OrderFactory.CreateApproved(); };
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_approved : approved_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Approve(theInitiatorId));

        public It should_retain_status_approved = () => sut.Status.ShouldEqual(OrderStatus.Approved);

        public It should_throw_order_already_approved =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyApprovedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_closed : approved_order_concern
    {
        public Because of = () => sut.Close(theInitiatorId);

        public It should_be_modified_by =
            () => sut.ModifiedBy.ShouldEqual(theInitiatorId);

        public It should_be_modified_on =
            () => sut.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Matches(p => p.OrderId == sut.Id)));
    }

    [Subject(typeof (Order))]
    public class when_a_approved_order_is_rejected : approved_order_concern
    {
        public Because of = () => sut.Reject(theInitiatorId);

        public It should_be_modified_by =
            () => sut.ModifiedBy.ShouldEqual(theInitiatorId);

        public It should_be_modified_on =
            () => sut.ModifiedUtc.Value.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Matches(p => p.OrderId == sut.Id)));
    }


    public abstract class rejected_order_concern : order_concern
    {
        public Establish ctx = () => { sut = OrderFactory.CreateRejected(); };
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_approved : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Approve(theInitiatorId));

        public It should_retain_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_rejected : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Reject(theInitiatorId));

        public It should_retain_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_rejected_order_is_closed : rejected_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Close(theInitiatorId));

        public It should_retain_status_rejected = () => sut.Status.ShouldEqual(OrderStatus.Rejected);

        public It should_throw_order_already_rejected =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyRejectedException>();
    }


    public abstract class closed_order_concern : order_concern
    {
        public Establish ctx = () => { sut = OrderFactory.CreateClosed(); };
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_approved : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Approve(theInitiatorId));

        public It should_retain_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_rejected : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Reject(theInitiatorId));

        public It should_retain_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }

    [Subject(typeof (Order))]
    public class when_a_closed_order_is_closed : closed_order_concern
    {
        public Because of = () => spec.catch_exception(() => sut.Close(theInitiatorId));

        public It should_retain_status_closed = () => sut.Status.ShouldEqual(OrderStatus.Closed);

        public It should_throw_order_already_closed =
            () => spec.exception_thrown.ShouldBeAn<OrderAlreadyClosedException>();
    }
    
    public class OrderFactory
    {
        public static Order CreatePending()
        {
            var lessor = new Lessor(new LessorId(), "OrderFactory");
            return new Order(lessor, BorrowerFactory.CreateLessee());
        }

        public static Order CreateClosed()
        {
            var result = CreatePending();
            result.Close(new UserId());
            return result;
        }

        public static Order CreateRejected()
        {
            var result = CreatePending();
            result.Reject(new UserId());
            return result;
        }

        public static Order CreateApproved()
        {
            var result = CreatePending();
            result.Approve(new UserId());
            return result;
        }

        private static class BorrowerFactory
        {
            public static Lessee CreateLessee()
            {
                return CreateLessee(Guid.NewGuid());
            }

            public static Lessee CreateLessee(Guid borrowerId)
            {
                return new Lessee(borrowerId, "Hans", "Muster", "Strasse", "6000", "Luzern", "hans.muster@test.phundus.ch",
                    "+4179123456", "");
            }

            public static Lessee CreateLessee(Guid borrowerId, string firstName, string lastName, string street = "",
                string postcode = "", string city = "", string emailAddress = "", string mobilePhoneNumber = "",
                string memberNumber = "")
            {
                return new Lessee(borrowerId, firstName, lastName, street, postcode, city, emailAddress, mobilePhoneNumber,
                    memberNumber);
            }
        }
    }
}