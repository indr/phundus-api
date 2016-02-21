namespace Phundus.Tests.Shop.Events
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderItemPeriodChanged))]
    public class order_item_period_changed : shop_domain_event_concern<OrderItemPeriodChanged>
    {
        private static OrderId theOrderId;
        private static ShortOrderId theShortOrderId;
        private static int theOrderStatus;
        private static decimal theOrderTotal;
        private static Guid theOrderItemId;
        private static Period theOldPeriod;
        private static Period theNewPeriod;
        private static OrderEventItem theOrderItem;

        private Establish ctx = () =>
        {
            theOrderId = new OrderId();
            theShortOrderId = new ShortOrderId(1234);
            theOrderStatus = 1;
            theOrderTotal = 12.50m;
            theOrderItemId = Guid.NewGuid();
            theOldPeriod = Period.FromNow(1);
            theNewPeriod = Period.FromNow(2);
            theOrderItem = CreateOrderEventItem();

            sut_factory.create_using(() =>
                new OrderItemPeriodChanged(theInitiator, theOrderId, theShortOrderId,
                    theOrderStatus, theOrderTotal, theOrderItemId, theOldPeriod,
                    theNewPeriod, theOrderItem));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_order_id = () =>
            dataMember(2).ShouldEqual(theOrderId.Id);

        private It should_have_at_3_the_short_order_id = () =>
            dataMember(3).ShouldEqual(theShortOrderId.Id);

        private It should_have_at_4_the_order_status = () =>
            dataMember(4).ShouldEqual(theOrderStatus);

        private It should_have_at_5_the_order_total = () =>
            dataMember(5).ShouldEqual(theOrderTotal);

        private It should_have_at_6_the_order_item_id = () =>
            dataMember(6).ShouldEqual(theOrderItemId);

        private It should_have_at_7_the_old_period = () =>
            dataMember(7).ShouldEqual(theOldPeriod);

        private It should_have_at_8_the_new_period = () =>
            dataMember(8).ShouldEqual(theNewPeriod);

        private It should_have_at_9_the_order_item = () =>
            dataMember(9).ShouldEqual(theOrderItem);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderItemPeriodChanged");
    }
}