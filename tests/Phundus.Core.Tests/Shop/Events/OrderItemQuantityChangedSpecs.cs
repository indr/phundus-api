namespace Phundus.Tests.Shop.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderItemQuantityChanged))]
    public class OrderItemQuantityChangedSpecs : shop_domain_event_concern<OrderItemQuantityChanged>
    {
        private static OrderEventLine theOrderItem;
        private static OrderId theOrderId = new OrderId();
        private static OrderShortId theOrderShortId = new OrderShortId(1234);
        private static OrderLineId theOrderLineId = new OrderLineId();

        private Establish ctx = () =>
        {
            theOrderItem = CreateOrderEventItem();
            sut_factory.create_using(() =>
                new OrderItemQuantityChanged(theManager, theOrderId, theOrderShortId, 2,
                    3.33m, theOrderLineId, 4, 5, theOrderItem));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());

        private It should_have_at_2_the_order_id = () =>
            dataMember(2).ShouldEqual(theOrderId.Id);

        private It should_have_at_3_the_order_short_id = () =>
            dataMember(3).ShouldEqual(theOrderShortId.Id);

        private It should_have_at_4_the_order_status = () =>
            dataMember(4).ShouldEqual(2);

        private It should_have_at_5_the_order_total = () =>
            dataMember(5).ShouldEqual(3.33m);

        private It should_have_at_6_the_order_line_id = () =>
            dataMember(6).ShouldEqual(theOrderLineId.Id);

        private It should_have_at_7_the_old_quantity = () =>
            dataMember(7).ShouldEqual(4);

        private It should_have_at_8_the_new_quantity = () =>
            dataMember(8).ShouldEqual(5);

        private It should_have_at_9_the_order_line = () =>
            dataMember(9).ShouldEqual(theOrderItem);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderItemQuantityChanged");
    }
}