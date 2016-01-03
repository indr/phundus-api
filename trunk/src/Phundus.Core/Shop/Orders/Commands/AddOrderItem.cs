namespace Phundus.Core.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAndAccess.Queries;
    using Repositories;
    using Shop.Services;

    public class AddOrderItem
    {
        public int OrderId { get; set; }
        public int InitiatorId { get; set; }
        public ArticleId ArticleId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Amount { get; set; }
        public Guid OrderItemId { get; set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        public IArticleService ArticleService { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            var article = ArticleService.GetById(new OwnerId(order.Lessor.LessorId.Id), command.ArticleId);
            
            MemberInRole.ActiveChief(order.Lessor.LessorId.Id, command.InitiatorId);

            var item = order.AddItem(article, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime(), command.Amount);

            command.OrderItemId = item.Id;
        }
    }
}