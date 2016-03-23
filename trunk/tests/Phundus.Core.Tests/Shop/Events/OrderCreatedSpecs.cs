namespace Phundus.Tests.Shop.Model
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Events;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderCreated))]
    public class order_created : shop_domain_event_concern<OrderCreated>
    {
        private static OrderShortId theOrderShortId;
        private static OrderId theOrderId;
        private static OrderStatus theOrderStatus;
        private static decimal theOrderTotal;
        private static IList<OrderEventLine> theOrderLines;

        private Establish ctx = () =>
        {
            theOrderShortId = new OrderShortId(1234);
            theOrderId = new OrderId();
            theOrderStatus = OrderStatus.Approved;
            theOrderTotal = 120.0m;
            theOrderLines = new List<OrderEventLine>();
            theOrderLines.Add(CreateOrderEventItem());

            sut_factory.create_using(() =>
                new OrderCreated(theInitiator.ToActor(), theOrderId, theOrderShortId,
                    theLessor, theLessee, theOrderStatus, theOrderTotal, theOrderLines));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_order_short_id = () =>
            dataMember(1).ShouldEqual(theOrderShortId.Id);

        private It should_have_at_2_the_order_id = () =>
            dataMember(2).ShouldEqual(theOrderId.Id);

        private It should_have_at_3_the_initiator = () =>
            dataMember(3).ShouldEqual(theInitiator.ToActor());

        private It should_have_at_4_the_lessor = () =>
            dataMember(4).ShouldEqual(theLessor);

        private It should_have_at_5_the_lessee = () =>
            dataMember(5).ShouldEqual(theLessee);

        private It should_have_at_6_the_order_status = () =>
            dataMember(6).ShouldEqual((int) theOrderStatus);

        private It should_have_at_7_the_order_total = () =>
            dataMember(7).ShouldEqual(theOrderTotal);

        private It should_have_at_8_the_order_lines = () =>
            dataMember(8).ShouldEqual(theOrderLines);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderCreated");
    }
}