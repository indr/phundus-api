namespace Phundus.Core.Tests.Shop
{
    using System;
    using Core.IdentityAndAccess.Users.Model;
    using Core.Inventory.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_item_is_handled : order_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private const int organizationId = 1;
        private const int initiatorId = 2;
        private const int orderId = 3;
        private const int articleId = 4;
        private static Order order;

        public Establish c = () =>
        {
            order = new Order(organizationId, new User());
            orders.setup(x => x.ById(orderId)).Return(order);
            articles.setup(x => x.ById(articleId)).Return(new Article(organizationId, "Artikel"));
            command = new AddOrderItem
            {
                Amount = 10,
                ArticleId = articleId,
                From = DateTime.Today,
                To = DateTime.Today.AddDays(1),
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_set_order_item_id =
            () => command.OrderItemId.ShouldNotEqual(Guid.Empty);

        public It should_publish_order_item_added =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAdded>.Is.NotNull));
    }
}