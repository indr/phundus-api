namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Application.Commands;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_item_is_handled : order_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private static UserId initiatorId = new UserId(2);
        private static OrderId orderId = new OrderId(3);
        private static ArticleId articleId = new ArticleId(4);
        private static Order order;

        public Establish c = () =>
        {
            order = new Order(organization, BorrowerFactory.Create());
            orders.setup(x => x.GetById(orderId.Id)).Return(order);
            articles.setup(x => x.GetById(organization.Id, articleId.Id))
                .Return(new Article(articleId, organization.Id, "Artikel"));
            command = new AddOrderItem
            {
                Quantity = 10,
                ArticleId = articleId,
                FromUtc = DateTime.UtcNow,
                ToUtc = DateTime.UtcNow.AddDays(1),
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrganizationId = organizationId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId.Id, initiatorId.Id));

        public It should_publish_order_item_added =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAdded>.Is.NotNull));

        public It should_set_order_item_id =
            () => command.ResultingOrderItemId.ShouldNotEqual(Guid.Empty);
    }
}