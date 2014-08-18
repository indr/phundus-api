namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Inventory;
    using Inventory.Articles;
    using Inventory.Articles.Repositories;
    using Repositories;

    public class AddOrderItem
    {
        public int OrderId { get; set; }
        public int InitiatorId { get; set; }
        public int ArticleId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
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
            var article = ArticleRepository.GetById(command.ArticleId);

            var order = OrderRepository.GetById(command.OrderId);
            
            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            var item = order.AddItem(article, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime(), command.Amount);

            command.OrderItemId = item.Id;
        }
    }
}