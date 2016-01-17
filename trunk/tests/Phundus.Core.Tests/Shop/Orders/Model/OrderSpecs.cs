namespace Phundus.Tests.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (Order))]
    public class when_creating_an_order_without_items : creating_order_concern
    {
        public Establish ctx = () =>
        {
            theLessor = new Lessor(new LessorId(), "Lessor");
            theLessee = CreateLessee();
        };

        public Because of = () => sut = new Order(theLessor, theLessee);

        public It should_be_empty = () => sut.Items.ShouldBeEmpty();

        public It should_have_status_pending =
            () => sut.Status.ShouldEqual(OrderStatus.Pending);

        public It should_have_the_borrower =
            () => sut.Lessee.ShouldEqual(theLessee);

        public It should_have_the_created_on_set_to_utc_now =
            () => sut.CreatedUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_the_organization =
            () => sut.Lessor.ShouldEqual(theLessor);

        public It should_not_have_a_modified_date =
            () => sut.ModifiedUtc.ShouldBeNull();

        public It should_not_have_a_modifier =
            () => sut.ModifiedBy.ShouldBeNull();
    }

    [Subject(typeof (Order))]
    public class when_creating_an_order_three_with_items : order_concern
    {
        private static List<OrderItem> theItems;

        public Establish ctx = () =>
        {
            theLessor = new Lessor(new LessorId(), "Lessor");
            theLessee = CreateLessee();

            theItems = new List<OrderItem>();
            theItems.Add(new OrderItem(null, new OrderItemId(), CreateArticle(1), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
            theItems.Add(new OrderItem(null, new OrderItemId(), CreateArticle(2), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
            theItems.Add(new OrderItem(null, new OrderItemId(), CreateArticle(3), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
        };

        public Because of = () => sut = new Order(theLessor, theLessee, theItems);

        private It should_copy_the_items = () => sut.Items.ShouldNotContain(theItems);
        private It should_have_three_items = () => sut.Items.Count.ShouldEqual(3);
        private It should_not_be_empty = () => sut.Items.ShouldNotBeEmpty();
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
            theArticle = CreateArticle(1);
            thePeriod = Period.FromNow(2);
        };

        private Because of = () => sut.AddItem(theOrderItemId, theArticle, thePeriod.FromUtc, thePeriod.ToUtc, theQuantity);

        private It should_have_an_order_item =
            () => sut.Items.ShouldContain(p => p.Id == theOrderItemId.Id && p.FromUtc == thePeriod.FromUtc && p.ToUtc == thePeriod.ToUtc);
    }
}