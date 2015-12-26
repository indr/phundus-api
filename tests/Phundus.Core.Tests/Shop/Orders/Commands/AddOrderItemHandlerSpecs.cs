namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using Core.Inventory.Articles.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;
    using Article = Core.Inventory.Articles.Model.Article;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_item_is_handled : order_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private const int initiatorId = 2;
        private const int orderId = 3;
        private const int articleId = 4;
        private static Order order;

        public Establish c = () =>
        {
            order = new Order(organization, BorrowerFactory.Create());
            orders.setup(x => x.GetById(orderId)).Return(order);
            articles.setup(x => x.GetById(organization.Id, articleId)).Return(new Article(organization.Id, "Artikel"));
            command = new AddOrderItem
            {
                Amount = 10,
                ArticleId = articleId,
                FromUtc = DateTime.UtcNow,
                ToUtc = DateTime.UtcNow.AddDays(1),
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organization.Id, initiatorId));

        public It should_set_order_item_id =
            () => command.OrderItemId.ShouldNotEqual(Guid.Empty);

        public It should_publish_order_item_added =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAdded>.Is.NotNull));
    }
}