namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_item_is_handled : order_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private const int initiatorId = 2;
        private const int orderId = 3;
        private static ArticleId articleId = new ArticleId(4);        
        private static Order order;

        public Establish c = () =>
        {
            var ownerId = new OwnerId(Guid.NewGuid());
            var owner = new Owner(ownerId, "Owner");
            lessor = new Lessor(new LessorId(owner.OwnerId.Id), "Lessor");
            order = new Order(lessor, BorrowerFactory.Create());
            orders.setup(x => x.GetById(orderId)).Return(order);
            var article = new Article(articleId.Id, owner, "Artikel", 1.0m);
            articles.setup(x => x.GetById(ownerId, articleId)).Return(article);
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
            () => memberInRole.WasToldTo(x => x.ActiveChief(lessor.LessorId.Id, initiatorId));

        public It should_publish_order_item_added =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAdded>.Is.NotNull));

        public It should_set_order_item_id =
            () => command.OrderItemId.ShouldNotEqual(Guid.Empty);
    }
}