namespace Phundus.Tests.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public abstract class order_concern : aggregate_concern_new<Order>
    {
        protected static shop_factory make;

        protected static Lessor theLessor;
        protected static Lessee theLessee;

        private Establish ctx = () =>
        {
            make = new shop_factory(fake);

            sut_factory.create_using(() =>
            {
                theLessor = make.Lessor();
                theLessee = make.Lessee();
                return new Order(theLessor, theLessee);
            });
        };
    }

    [Subject(typeof (Order))]
    public class when_creating_an_order_without_items : order_concern
    {
        private Establish ctx = () => sut_factory.create_using(() =>
        {
            theLessor = make.Lessor();
            theLessee = make.Lessee();
            return new Order(theLessor, theLessee);
        });

        public It should_be_empty = () => sut.Items.ShouldBeEmpty();

        public It should_have_status_pending =
            () => sut.Status.ShouldEqual(OrderStatus.Pending);

        public It should_have_the_borrower =
            () => sut.Lessee.ShouldEqual(theLessee);

        public It should_have_the_created_on_set_to_utc_now =
            () => sut.CreatedUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_the_lessor =
            () => sut.Lessor.ShouldEqual(theLessor);

        public It should_not_have_a_modified_date =
            () => sut.ModifiedUtc.ShouldBeNull();
    }

    [Subject(typeof (Order))]
    public class when_creating_an_order_three_with_items : order_concern
    {
        private static List<OrderItem> theItems;

        public Establish ctx = () =>
        {
            theItems = new List<OrderItem>();
            theItems.Add(new OrderItem(null, new OrderItemId(), make.Article(), DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1), 1));
            theItems.Add(new OrderItem(null, new OrderItemId(), make.Article(), DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1), 1));
            theItems.Add(new OrderItem(null, new OrderItemId(), make.Article(), DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1), 1));

            sut_factory.create_using(() =>
            {
                theLessor = make.Lessor();
                theLessee = make.Lessee();
                return new Order(theLessor, theLessee, theItems);
            });
        };

        private It should_copy_the_items = () =>
            sut.Items.ShouldNotContain(theItems);

        private It should_have_three_items = () =>
            sut.Items.Count.ShouldEqual(3);

        private It should_not_be_empty = () =>
            sut.Items.ShouldNotBeEmpty();
    }

    [Subject(typeof (Order))]
    public class when_adding_an_order_item : order_concern
    {
        private static Article theArticle;
        private static Period thePeriod;
        private static int theQuantity = 10;
        private static OrderItemId theOrderItemId;

        private Establish ctx = () =>
        {
            theOrderItemId = new OrderItemId();
            theArticle = make.Article();
            thePeriod = Period.FromNow(2);
        };

        private Because of =
            () => sut.AddItem(theOrderItemId, theArticle, thePeriod.FromUtc, thePeriod.ToUtc, theQuantity);

        private It should_have_an_order_item =
            () =>
                sut.Items.ShouldContain(
                    p => p.Id == theOrderItemId.Id && p.FromUtc == thePeriod.FromUtc && p.ToUtc == thePeriod.ToUtc);
    }
}