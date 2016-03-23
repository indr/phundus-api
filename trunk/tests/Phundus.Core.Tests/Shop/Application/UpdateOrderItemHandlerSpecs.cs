namespace Phundus.Tests.Shop.Application
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;

    [Subject(typeof (UpdateOrderItemHandler))]
    public class when_update_order_command_item_is_handled :
        order_command_handler_concern<UpdateOrderItem, UpdateOrderItemHandler>
    {
        private const int newQuantity = 20;
        private static Order theOrder;
        private static DateTime newFromUtc;
        private static DateTime newToUtc;
        private static OrderLineId theOrderItemId;

        public Establish c = () =>
        {
            var article = make.Product();
            theOrder = new Order(theManager, new OrderId(), new OrderShortId(1234), theLessor, theLessee);
            theOrderItemId = new OrderLineId();
            theOrder.AddItem(theManager, theOrderItemId, article, Period.FromNow(1), 1, 1);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            newFromUtc = DateTime.UtcNow.AddDays(1);
            newToUtc = DateTime.UtcNow.AddDays(2);

            command = new UpdateOrderItem(theInitiatorId, theOrder.OrderId, theOrderItemId.Id,
                new Period(newFromUtc, newToUtc), newQuantity, 0.0m);
        };

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));

        public It should_update_order_items_quantity = () =>
            theOrder.Lines.Single(p => p.LineId.Id == theOrderItemId.Id).Quantity.ShouldEqual(newQuantity);

        public It should_update_order_items_period_from_to_midnight = () =>
            theOrder.Lines.Single(p => p.LineId.Id == theOrderItemId.Id)
                .Period.FromUtc.ShouldEqual(newFromUtc.ToLocalTime().Date.ToUniversalTime());

        public It should_update_order_items_period_to_one_second_before_midnight = () =>
            theOrder.Lines.Single(p => p.LineId.Id == theOrderItemId.Id)
                .Period.ToUtc.ShouldEqual(newToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());
    }
}