namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (UpdateOrderItemHandler))]
    public class when_update_order_command_item_is_handled :
        order_command_handler_concern<UpdateOrderItem, UpdateOrderItemHandler>
    {
        private const int newAmount = 20;
        private static Order theOrder;
        private static DateTime newFromUtc;
        private static DateTime newToUtc;
        private static OrderItemId theOrderItemId;

        public Establish c = () =>
        {
            var article = make.Article();
            theOrder = new Order(theInitiator, new OrderId(), new OrderShortId(1234), theLessor, CreateLessee());
            theOrderItemId = new OrderItemId();
            theOrder.AddItem(theInitiator, theOrderItemId, article, DateTime.Today, DateTime.Today, 1);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            newFromUtc = DateTime.UtcNow.AddDays(1);
            newToUtc = DateTime.UtcNow.AddDays(2);
            command = new UpdateOrderItem
            {
                InitiatorId = theInitiatorId,
                OrderId = theOrder.OrderId,
                OrderItemId = theOrderItemId.Id,
                Amount = newAmount,
                FromUtc = newFromUtc,
                ToUtc = newToUtc
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));

        public It should_update_order_items_amount = () =>
            theOrder.Items.Single(p => p.ItemId.Id == theOrderItemId.Id).Amount.ShouldEqual(newAmount);

        public It should_update_order_items_period_from_to_midnight = () =>
            theOrder.Items.Single(p => p.ItemId.Id == theOrderItemId.Id)
                .FromUtc.ShouldEqual(newFromUtc.ToLocalTime().Date.ToUniversalTime());

        public It should_update_order_items_period_to_one_second_before_midnight = () =>
            theOrder.Items.Single(p => p.ItemId.Id == theOrderItemId.Id)
                .ToUtc.ShouldEqual(newToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());
    }
}