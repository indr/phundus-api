namespace Phundus.Shop.Orders.Commands
{
    using System;
    using System.Data.Odbc;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Repositories;
    using Shop.Services;

    public class AddOrderItem : ICommand
    {
        public AddOrderItem(InitiatorId initiatorId, ShortOrderId shortOrderId, OrderItemId orderItemId, ArticleShortId articleShortId, Period period, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (shortOrderId == null) throw new ArgumentNullException("shortOrderId");
            if (orderItemId == null) throw new ArgumentNullException("orderItemId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (period == null) throw new ArgumentNullException("period");            
            InitiatorId = initiatorId;
            ShortOrderId = shortOrderId;
            OrderItemId = orderItemId;
            ArticleShortId = articleShortId;
            Period = period;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ShortOrderId ShortOrderId { get; protected set; }
        public OrderItemId OrderItemId { get; protected set; }
        public ArticleShortId ArticleShortId { get; protected set; }
        public Period Period { get; protected set; }
        public int Quantity { get; protected set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        private readonly IArticleService _articleService;
        private readonly IMemberInRole _memberInRole;
        private readonly IOrderRepository _orderRepository;

        public AddOrderItemHandler(IArticleService articleService, IMemberInRole memberInRole, IOrderRepository orderRepository)
        {
            if (articleService == null) throw new ArgumentNullException("articleService");
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            _articleService = articleService;
            _memberInRole = memberInRole;
            _orderRepository = orderRepository;
        }

        public void Handle(AddOrderItem command)
        {
            var order = _orderRepository.GetById(command.ShortOrderId.Id);
            var lessor = order.Lessor;
            _memberInRole.ActiveManager(lessor.LessorId.Id, command.InitiatorId);

            var article = _articleService.GetById(lessor.LessorId, command.ArticleShortId, order.Lessee.LesseeId);
            order.AddItem(command.OrderItemId, article, command.Period.FromUtc, command.Period.ToUtc, command.Quantity);
        }
    }
}