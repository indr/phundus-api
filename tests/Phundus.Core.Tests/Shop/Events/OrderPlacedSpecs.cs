﻿namespace Phundus.Tests.Shop.Events
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderPlaced))]
    public class order_placed : domain_event_concern<OrderPlaced>
    {
        private static ShortOrderId theShortOrderId;
        private static Lessor theLessor;
        private static OrderId theOrderId;
        private static Lessee theLessee;
        private static int theStatus = 1;
        private static decimal theTotalPrice = 10.20m;
        private static List<OrderEventItem> theItems;

        private Establish ctx = () =>
        {
            theShortOrderId = new ShortOrderId(1234);
            theOrderId = new OrderId();
            theLessor = new Lessor(new LessorId(), "The lessor", true);
            theLessee = new Lessee(new LesseeId(), "First name", "Last name", "Street", "Postcode", "City",
                "Email address", "Phone number", "Member number");
            theItems = new List<OrderEventItem>();
            theItems.Add(new OrderEventItem(Guid.NewGuid(), new ArticleId(), new ArticleShortId(1234),
                "The text", 1.23m, DateTime.Today, DateTime.Today.AddDays(1), 10, 12.3m));

            sut_factory.create_using(() =>
                new OrderPlaced(theInitiator, theOrderId, theShortOrderId,
                    theLessor, theLessee, theStatus, theTotalPrice, theItems));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_short_order_id = () =>
            dataMember(1).ShouldEqual(theShortOrderId.Id);

        private It should_have_at_2_the_lessor_id = () =>
            dataMember(2).ShouldEqual(theLessor.LessorId.Id);

        private It should_have_at_3_the_order_id = () =>
            dataMember(3).ShouldEqual(theOrderId.Id);

        private It should_have_at_4_the_initiator = () =>
            dataMember(4).ShouldEqual(theInitiator);

        private It should_have_at_5_the_lessor = () =>
            dataMember(5).ShouldEqual(theLessor);

        private It should_have_at_6_the_lessee = () =>
            dataMember(6).ShouldEqual(theLessee);

        private It should_have_at_7_the_status = () =>
            dataMember(7).ShouldEqual(theStatus);

        private It should_have_at_8_the_total_price = () =>
            dataMember(8).ShouldEqual(theTotalPrice);

        private It should_have_at_9_the_items = () =>
            dataMember(9).ShouldBeLike(theItems);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderPlaced");
    }
}