namespace Phundus.Tests.Shop.Events
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderItemTotalChanged))]
    public class order_item_total_changed : shop_domain_event_concern<OrderItemTotalChanged>
    {
        private static OrderId theOrderId;
        private static OrderShortId theOrderShortId;
        private static int theOrderStatus;
        private static decimal theOrderTotal;
        private static OrderLineId theOrderItemId;
        private static decimal theOldItemTotal;
        private static decimal theNewItemTotal;
        private static OrderEventItem theOrderItem;

        private Establish ctx = () =>
        {
            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);
            theOrderStatus = 1;
            theOrderTotal = 12.50m;
            theOrderItemId = new OrderLineId();
            theOldItemTotal = 1.50m;
            theNewItemTotal = 1.60m;
            theOrderItem = CreateOrderEventItem();

            sut_factory.create_using(() =>
                new OrderItemTotalChanged(theInitiator, theOrderId, theOrderShortId,
                    theOrderStatus, theOrderTotal, theOrderItemId, theOldItemTotal,
                    theNewItemTotal, theOrderItem));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_order_id = () =>
            dataMember(2).ShouldEqual(theOrderId.Id);

        private It should_have_at_3_the_order_short_id = () =>
            dataMember(3).ShouldEqual(theOrderShortId.Id);

        private It should_have_at_4_the_order_status = () =>
            dataMember(4).ShouldEqual(theOrderStatus);

        private It should_have_at_5_the_order_total = () =>
            dataMember(5).ShouldEqual(theOrderTotal);

        private It should_have_at_6_the_order_item_id = () =>
            dataMember(6).ShouldEqual(theOrderItemId.Id);

        private It should_have_at_7_the_old_item_total = () =>
            dataMember(7).ShouldEqual(theOldItemTotal);

        private It should_have_at_8_the_new_item_total = () =>
            dataMember(8).ShouldEqual(theNewItemTotal);

        private It should_have_at_9_the_order_item = () =>
            dataMember(9).ShouldEqual(theOrderItem);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderItemTotalChanged");
    }
}