namespace Phundus.Tests.Shop.Events
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderEventItem))]
    public class order_event_item : domain_event_concern<OrderEventItem>
    {
        private static OrderLineId theItemId = new OrderLineId();
        private static ArticleId theArticleId = new ArticleId();
        private static ArticleShortId theArticleShortId = new ArticleShortId(1234);
        private static string theText = "The text";
        private static decimal theUnitPricePerWeek = 1.23m;
        private static int theQuantity = 2;
        private static Period thePeriod = Period.FromNow(6);
        private static decimal theItemTotal = 2.50m;

        private Establish ctx = () => sut_factory.create_using(() =>
            new OrderEventItem(theItemId, theArticleId, theArticleShortId,
                theText, theUnitPricePerWeek, thePeriod, theQuantity,
                theItemTotal));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_item_id = () =>
            dataMember(1).ShouldEqual(theItemId.Id);

        private It should_have_at_2_the_article_id = () =>
            dataMember(2).ShouldEqual(theArticleId.Id);

        private It should_have_at_3_the_article_short_id = () =>
            dataMember(3).ShouldEqual(theArticleShortId.Id);

        private It should_have_at_4_the_text = () =>
            dataMember(4).ShouldEqual(theText);

        private It should_have_at_5_the_unit_price_per_week = () =>
            dataMember(5).ShouldEqual(theUnitPricePerWeek);

        private It should_have_at_6_the_from_utc = () =>
            dataMember(6).ShouldEqual(thePeriod.FromUtc);

        private It should_have_at_7_the_to_utc = () =>
            dataMember(7).ShouldEqual(thePeriod.ToUtc);

        private It should_have_at_8_the_quantity = () =>
            dataMember(8).ShouldEqual(theQuantity);

        private It should_have_at_9_the_item_total = () =>
            dataMember(9).ShouldEqual(theItemTotal);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderEventItem");
    }
}