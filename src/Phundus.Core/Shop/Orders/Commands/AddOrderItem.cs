namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;
    using Shop.Services;

    public class AddOrderItem : ICommand
    {
        public AddOrderItem(InitiatorGuid initiatorId, OrderId orderId, OrderItemId orderItemId, ArticleId articleId, Period period, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderItemId == null) throw new ArgumentNullException("orderItemId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (period == null) throw new ArgumentNullException("period");            
            InitiatorId = initiatorId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            ArticleId = articleId;
            Period = period;
            Quantity = quantity;
        }

        public InitiatorGuid InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public OrderItemId OrderItemId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public Period Period { get; protected set; }
        public int Quantity { get; protected set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        public IArticleService ArticleService { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(AddOrderItem command)
        {
            var order = OrderRepository.GetById(command.OrderId.Id);
            var lessor = order.Lessor;
            MemberInRole.ActiveChief(lessor.LessorId.Id, command.InitiatorId);

            var article = ArticleService.GetById(lessor.LessorId, command.ArticleId);
            order.AddItem(article, command.Period.FromUtc, command.Period.ToUtc, command.Quantity);
        }
    }
}