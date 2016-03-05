namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

    public class AddOrderItem : ICommand
    {
        public AddOrderItem(InitiatorId initiatorId, OrderId orderId, OrderLineId orderLineId, ArticleId articleId,
            Period period, int quantity)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderLineId == null) throw new ArgumentNullException("orderLineId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (period == null) throw new ArgumentNullException("period");

            InitiatorId = initiatorId;
            OrderId = orderId;
            OrderLineId = orderLineId;
            ArticleId = articleId;
            Period = period;
            Quantity = quantity;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public OrderLineId OrderLineId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public Period Period { get; protected set; }
        public int Quantity { get; protected set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        private readonly IArticleService _articleService;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserInRole _userInRole;

        public AddOrderItemHandler(IUserInRole userInRole, IOrderRepository orderRepository,
            IArticleService articleService)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            if (articleService == null) throw new ArgumentNullException("articleService");

            _userInRole = userInRole;
            _orderRepository = orderRepository;
            _articleService = articleService;
        }

        [Transaction]
        public void Handle(AddOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);
            var article = _articleService.GetById(order.Lessor.LessorId, command.ArticleId, order.Lessee.LesseeId);

            order.AddItem(manager, command.OrderLineId, article, command.Period, command.Quantity);

            _orderRepository.Save(order);
        }
    }
}