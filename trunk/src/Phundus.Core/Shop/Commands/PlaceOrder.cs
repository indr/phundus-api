namespace Phundus.Shop.Orders.Commands
{
    using System;
    using System.Linq;
    using Authorization;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Integration.IdentityAccess;
    using Model;
    using Phundus.Authorization;
    using Repositories;
    using Shop.Services;

    public class PlaceOrder : ICommand
    {
        public PlaceOrder(InitiatorId initiatorId, LessorId lessorId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            InitiatorId = initiatorId;
            LessorId = lessorId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public LessorId LessorId { get; protected set; }
        public int ResultingOrderId { get; set; }
    }

    public class PlaceOrderHandler : IHandleCommand<PlaceOrder>
    {
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;
        private readonly ICartRepository _cartRepository;
        private readonly ILesseeService _lesseeService;
        private readonly IArticleService _articleService;
        private readonly ILessorService _lessorService;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderHandler(IAuthorize authorize, IInitiatorService initiatorService, ICartRepository cartRepository, IOrderRepository orderRepository,
            ILessorService lessorService, ILesseeService lesseeService, IArticleService articleService)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            if (lessorService == null) throw new ArgumentNullException("lessorService");
            if (lesseeService == null) throw new ArgumentNullException("lesseeService");
            if (articleService == null) throw new ArgumentNullException("articleService");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _lesseeService = lesseeService;
            _articleService = articleService;
        }

        public void Handle(PlaceOrder command)
        {
            var cart = _cartRepository.GetByUserGuid(new UserId(command.InitiatorId.Id));
            AssertionConcern.AssertArgumentFalse(cart.IsEmpty, "Your cart is empty, there is no order to place.");

            var cartItemsToPlace = cart.Items.Where(p => Equals(p.Article.LessorId, command.LessorId)).ToList();
            AssertionConcern.AssertArgumentGreaterThan(cartItemsToPlace.Count, 0,
                String.Format("The cart does not contain items belonging to the lessor {0}.", command.LessorId));

            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var lessor = _lessorService.GetById(command.LessorId);
            var lessee = _lesseeService.GetById(new LesseeId(command.InitiatorId.Id));

            foreach (var eachCartItem in cartItemsToPlace)
            {
                var article = _articleService.GetById(lessor.LessorId, eachCartItem.ArticleShortId, lessee.LesseeId);
                _authorize.Enforce(cart.UserId, Rent.Article(article));
            }

            var orderItems = cartItemsToPlace.Select(s => new OrderItem(s.ArticleShortId, s.LineText, new Period(s.From, s.To), s.Quantity, s.UnitPrice)).ToList();
            var order = new Order(lessor, lessee, orderItems);

            var orderId = _orderRepository.Add(order);
            command.ResultingOrderId = orderId;

            foreach (var each in cartItemsToPlace)
            {
                cart.RemoveItem(each.CartItemId);
            }

            EventPublisher.Publish(new OrderPlaced(initiator, order.OrderId, order.ShortOrderId,
                lessor, lessee));
        }
    }
}