namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Inventory;
    using Inventory.Repositories;
    using Repositories;

    public class AddOrderItem
    {
        public int OrderId { get; set; }
        public int InitiatorId { get; set; }
        public int ArticleId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Amount { get; set; }
        public Guid OrderItemId { get; set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        // TODO: Use integration service
        public IArticleRepository ArticleRepository { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddOrderItem command)
        {
            var article = ArticleRepository.ById(command.ArticleId);
            if (article == null)
                throw new ArticleNotFoundException();

            var order = OrderRepository.ById(command.OrderId);
            if (order == null)
                throw new OrderNotFoundException();

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            var item = order.AddItem(article, command.From.ToUniversalTime(), command.To.ToUniversalTime(), command.Amount);

            command.OrderItemId = item.Id;
        }
    }
}