﻿namespace Phundus.Tests.Shop.Events
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderPlaced))]
    public class order_placed : shop_domain_event_concern<OrderPlaced>
    {
        private static OrderShortId theOrderShortId;
        private static OrderId theOrderId;
        private static OrderStatus theOrderStatus;
        private static decimal theOrderTotal;
        private static List<OrderEventLine> theItems;

        private Establish ctx = () =>
        {
            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);
            theOrderStatus = OrderStatus.Closed;
            theOrderTotal = 12.34m;
            theItems = new List<OrderEventLine>();
            theItems.Add(CreateOrderEventItem());

            sut_factory.create_using(() =>
                new OrderPlaced(theInitiator, theOrderId, theOrderShortId,
                    theLessor, theLessee, theOrderStatus, theOrderTotal, theItems));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_order_short_id = () =>
            dataMember(1).ShouldEqual(theOrderShortId.Id);

        private It should_have_at_2_the_lessor_id = () =>
            dataMember(2).ShouldEqual(theLessor.LessorId.Id);

        private It should_have_at_3_the_order_id = () =>
            dataMember(3).ShouldEqual(theOrderId.Id);

        private It should_have_at_4_the_initiator = () =>
            dataMember(4).ShouldEqual(theInitiator.ToActor());

        private It should_have_at_5_the_lessor = () =>
            dataMember(5).ShouldEqual(theLessor);

        private It should_have_at_6_the_lessee = () =>
            dataMember(6).ShouldEqual(theLessee);

        private It should_have_at_7_the_status = () =>
            dataMember(7).ShouldEqual((int)theOrderStatus);

        private It should_have_at_8_the_total_price = () =>
            dataMember(8).ShouldEqual(theOrderTotal);

        private It should_have_at_9_the_items = () =>
            dataMember(9).ShouldBeLike(theItems);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderPlaced");
    }
}