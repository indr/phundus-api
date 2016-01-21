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
    using Rhino.Mocks;

    [Subject(typeof (UpdateOrderItemHandler))]
    public class when_update_order_command_item_is_handled : order_command_handler_concern<UpdateOrderItem, UpdateOrderItemHandler>
    {
        private const int orderId = 2;
        private const int newAmount = 20;
        private static UserId initiatorId = new UserId();        
        private static Order order;
        private static DateTime newFromUtc;
        private static DateTime newToUtc;
        private static OrderItemId theOrderItemId;

        public Establish c = () =>
        {
            var article = new Article(1, new Owner(new OwnerId(theLessor.LessorId.Id), "Owner"), "Artikel", 1.0m);
            order = new Order(theLessor, CreateLessee());
            theOrderItemId = new OrderItemId();
            order.AddItem(theOrderItemId, article, DateTime.Today, DateTime.Today, 1);
            orderRepository.setup(x => x.GetById(orderId)).Return(order);

            newFromUtc = DateTime.UtcNow.AddDays(1);
            newToUtc = DateTime.UtcNow.AddDays(2);
            command = new UpdateOrderItem
            {
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrderItemId = theOrderItemId.Id,
                Amount = newAmount,
                FromUtc = newFromUtc,
                ToUtc = newToUtc
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, initiatorId));

        public It should_publish_order_item_amount_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAmountChanged>.Is.NotNull));

        public It should_publish_order_item_period_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemPeriodChanged>.Is.NotNull));

        public It should_update_order_items_amount =
            () => order.Items.Single(p => p.Id == theOrderItemId.Id).Amount.ShouldEqual(newAmount);

        public It should_update_order_items_period_from_to_midnight =
            () =>
                order.Items.Single(p => p.Id == theOrderItemId.Id)
                    .FromUtc.ShouldEqual(newFromUtc.ToLocalTime().Date.ToUniversalTime());

        public It should_update_order_items_period_to_one_second_before_midnight =
            () =>
                order.Items.Single(p => p.Id == theOrderItemId.Id)
                    .ToUtc.ShouldEqual(newToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());
    }
}