namespace Phundus.Tests.Shop.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Products;
    using Phundus.Shop.Orders.Model;

    public abstract class order_concern : aggregate_root_concern<Order>
    {
        protected static shop_factory make;

        protected static Manager theManager;
        protected static Lessor theLessor;
        protected static Lessee theLessee;

        protected static OrderId theOrderId;

        private Establish ctx = () =>
        {
            make = new shop_factory(fake);

            theManager = make.Manager();
            theLessor = make.Lessor();
            theLessee = make.Lessee();
            theOrderId = new OrderId();

            sut_factory.create_using(() =>
                new Order(theInitiator, theOrderId, new OrderShortId(1234), theLessor, theLessee));
        };
    }

    [Subject(typeof (Order))]
    public class when_creating_an_order_without_items : order_concern
    {
        private Establish ctx = () => sut_factory.create_using(() =>
        {
            theLessor = make.Lessor();
            theLessee = make.Lessee();
            return new Order(theInitiator, new OrderId(), new OrderShortId(1234), theLessor, theLessee);
        });

        public It should_be_empty = () =>
            sut.Lines.ShouldBeEmpty();

        public It should_have_status_pending =
            () => sut.Status.ShouldEqual(OrderStatus.Pending);

        public It should_have_the_borrower =
            () => sut.Lessee.ShouldEqual(theLessee);

        public It should_have_the_created_on_set_to_utc_now =
            () => sut.CreatedAtUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_the_lessor =
            () => sut.Lessor.ShouldEqual(theLessor);
    }

    [Subject(typeof (Order))]
    public class when_adding_an_order_item : order_concern
    {
        private static Article theArticle;
        private static Period thePeriod;
        private static int theQuantity;
        private static OrderLineId theOrderItemId;
        private static decimal theLineTotal;

        private Establish ctx = () =>
        {
            theOrderItemId = new OrderLineId();
            theArticle = make.Product();
            thePeriod = Period.FromNow(6);
            theQuantity = 10;
            theLineTotal = 1.23m;
        };

        private Because of = () =>
            sut.AddItem(theManager, theOrderItemId, theArticle, thePeriod, theQuantity, theLineTotal);

        private It should_have_an_order_item = () =>
            sut.Lines.ShouldContain(p =>
                p.LineId.Id == theOrderItemId.Id
                && Equals(p.Period, thePeriod));

        private It should_have_mutating_event_order_item_added = () =>
            mutatingEvent<OrderItemAdded>(p =>
                p.OrderId == theOrderId.Id
                && p.OrderTotal == theLineTotal
                && p.OrderLine.LineId == theOrderItemId.Id
                && p.OrderLine.LineTotal == theLineTotal);
    }

    [Subject(typeof (Order))]
    public class when_placing_an_order : order_concern
    {
        private Because of = () =>
            sut.Place(theInitiator);

        private It should_have_mutating_event_order_placed = () =>
            mutatingEvent<OrderPlaced>(p =>
                p.OrderId == theOrderId.Id);
    }
}