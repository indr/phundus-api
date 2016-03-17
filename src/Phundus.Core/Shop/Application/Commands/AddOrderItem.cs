namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Collaborators;

    public class AddOrderItem : ICommand
    {
        public AddOrderItem(InitiatorId initiatorId, OrderId orderId, OrderLineId orderLineId, ArticleId articleId,
            Period period, int quantity, decimal lineTotal)
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
            LineTotal = lineTotal;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public OrderLineId OrderLineId { get; protected set; }
        public ArticleId ArticleId { get; protected set; }
        public Period Period { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal LineTotal { get; protected set; }
    }

    public class AddOrderItemHandler : IHandleCommand<AddOrderItem>
    {
        private readonly IArticleService _articleService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICollaboratorService _collaboratorService;

        public AddOrderItemHandler(ICollaboratorService collaboratorService, IOrderRepository orderRepository,
            IArticleService articleService)
        {
            _collaboratorService = collaboratorService;
            _orderRepository = orderRepository;
            _articleService = articleService;
        }

        [Transaction]
        public void Handle(AddOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _collaboratorService.Manager(order.Lessor.LessorId, command.InitiatorId);
            var article = _articleService.GetById(order.Lessor.LessorId, command.ArticleId, order.Lessee.LesseeId);

            order.AddItem(manager, command.OrderLineId, article, command.Period, command.Quantity, command.LineTotal);

            _orderRepository.Save(order);
        }
    }
}