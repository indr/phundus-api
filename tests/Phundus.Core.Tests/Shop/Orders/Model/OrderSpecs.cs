namespace Phundus.Tests.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Contracts.Model;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (Order))]
    public class when_creating_an_order_without_items : order_concern
    {
        private static Lessor lessor;
        private static Lessee lessee;

        public Establish ctx = () =>
        {
            lessor = new Lessor(new LessorId(), "Lessor");
            lessee = CreateLessee();
        };

        public Because of = () => { order = new Order(lessor, lessee); };
        public It should_be_empty = () => order.Items.ShouldBeEmpty();

        public It should_have_status_pending =
            () => order.Status.ShouldEqual(OrderStatus.Pending);

        public It should_have_the_borrower =
            () => order.Lessee.ShouldEqual(lessee);

        public It should_have_the_created_on_set_to_utc_now =
            () => order.CreatedUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        public It should_have_the_organization =
            () => order.Lessor.ShouldEqual(lessor);

        public It should_not_have_a_modified_date =
            () => order.ModifiedUtc.ShouldBeNull();

        public It should_not_have_a_modifier =
            () => order.ModifiedBy.ShouldBeNull();
    }

    [Subject(typeof (Order))]
    public class when_creating_an_order_three_with_items : order_concern
    {
        private static Lessor lessor;
        private static Lessee lessee;
        private static List<OrderItem> items;
        private static Owner theArticleOwner;

        public Establish ctx = () =>
        {
            lessor = new Lessor(new LessorId(), "Lessor");
            lessee = CreateLessee();
            theArticleOwner = new Owner(new OwnerId(), "The article owner");

            items = new List<OrderItem>();
            items.Add(new OrderItem(null, CreateArticle(1), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
            items.Add(new OrderItem(null, CreateArticle(2), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
            items.Add(new OrderItem(null, CreateArticle(3), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1));
        };

        public Because of = () => { order = new Order(lessor, lessee, items); };

        private static Article CreateArticle(int articleId)
        {
            return new Article(articleId, theArticleOwner, "Article " + articleId, 7.0m);
        }

        private It should_not_be_empty = () => order.Items.ShouldNotBeEmpty();

        private It should_have_three_items = () => order.Items.Count.ShouldEqual(3);

        private It should_copy_the_items = () => order.Items.ShouldNotContain(items);
    }
}