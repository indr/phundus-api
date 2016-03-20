namespace Phundus.Tests.Shop.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (OrderItemAdded))]
    public class order_item_added : shop_domain_event_concern<OrderItemAdded>
    {
        private static OrderId theOrderId;
        private static OrderShortId theOrderShortId;
        private static int theOrderStatus;
        private static decimal theOrderTotal;
        private static OrderEventLine theOrderItem;

        private Establish ctx = () =>
        {
            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);
            theOrderStatus = 123;
            theOrderTotal = 12.50m;
            theOrderItem = CreateOrderEventItem();

            sut_factory.create_using(() =>
                new OrderItemAdded(theManager, theOrderId, theOrderShortId,
                    theOrderStatus, theOrderTotal, theOrderItem));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_manager = () =>
            dataMember(1).ShouldEqual(theManager.ToActor());  

        private It should_have_at_2_the_order_id = () =>
            dataMember(2).ShouldEqual(theOrderId.Id);

        private It should_have_at_3_the_order_short_id  = () =>
            dataMember(3).ShouldEqual(theOrderShortId.Id);

        private It should_have_at_4_the_order_status = () =>
            dataMember(4).ShouldEqual(theOrderStatus);

        private It should_have_at_5_the_order_total = () =>
            dataMember(5).ShouldEqual(theOrderTotal);

        private It should_have_at_6_the_order_item = () =>
            dataMember(6).ShouldEqual(theOrderItem);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Orders.Model.OrderItemAdded");
    }
}