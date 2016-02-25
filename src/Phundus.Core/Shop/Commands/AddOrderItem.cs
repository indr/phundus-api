namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Repositories;
    using Shop.Services;

    public class AddOrderItem : ICommand
    {
        public AddOrderItem(InitiatorId initiatorId, ShortOrderId shortOrderId, OrderItemId orderItemId, ArticleId articleId, Period period, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (shortOrderId == null) throw new ArgumentNullException("shortOrderId");
            if (orderItemId == null) throw new ArgumentNullException("orderItemId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (period == null) throw new ArgumentNullException("period");            
            InitiatorId = initiatorId;
            ShortOrderId = shortOrderId;
            OrderItemId = orderItemId;
            ArticleId = articleId;
            Period = period;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public ShortOrderId ShortOrderId { get; protected set; }
        public OrderItemId OrderItemId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public Period Period { get; protected set; }
        public int Quantity { get; protected set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        private readonly IInitiatorService _initiatorService;
        private readonly IArticleService _articleService;
        private readonly IMemberInRole _memberInRole;
        private readonly IOrderRepository _orderRepository;

        public AddOrderItemHandler(IInitiatorService initiatorService, IArticleService articleService, IMemberInRole memberInRole, IOrderRepository orderRepository)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (articleService == null) throw new ArgumentNullException("articleService");
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            _initiatorService = initiatorService;
            _articleService = articleService;
            _memberInRole = memberInRole;
            _orderRepository = orderRepository;
        }

        public void Handle(AddOrderItem command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var order = _orderRepository.GetById(command.ShortOrderId.Id);
            var lessor = order.Lessor;
            _memberInRole.ActiveManager(lessor.LessorId.Id, command.InitiatorId);

            var article = _articleService.GetById(lessor.LessorId, command.ArticleId, order.Lessee.LesseeId);
            order.AddItem(initiator, command.OrderItemId, article, command.Period.FromUtc, command.Period.ToUtc, command.Quantity);
        }
    }
}