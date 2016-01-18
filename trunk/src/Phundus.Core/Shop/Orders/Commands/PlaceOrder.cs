namespace Phundus.Shop.Orders.Commands
{
    using System;
    using System.Linq;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Model;
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
        private readonly ICartRepository _cartRepository;
        private readonly ILesseeService _lesseeService;
        private readonly ILessorService _lessorService;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderHandler(ICartRepository cartRepository, IOrderRepository orderRepository,
            ILessorService lessorService, ILesseeService lesseeService)
        {
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            if (lessorService == null) throw new ArgumentNullException("lessorService");
            if (lesseeService == null) throw new ArgumentNullException("lesseeService");
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _lesseeService = lesseeService;
        }

        public void Handle(PlaceOrder command)
        {
            var cart = _cartRepository.GetByUserGuid(new UserId(command.InitiatorId.Id));
            AssertionConcern.AssertArgumentFalse(cart.IsEmpty, "Your cart is empty, there is no order to place.");

            var cartItemsToPlace = cart.Items.Where(p => p.Article.Owner.OwnerId.Id == command.LessorId.Id).ToList();
            AssertionConcern.AssertArgumentGreaterThan(cartItemsToPlace.Count, 0,
                String.Format("The cart does not contain items belonging to the lessor {0}.", command.LessorId));

            var lessor = _lessorService.GetById(command.LessorId);
            var lessee = _lesseeService.GetById(new LesseeId(command.InitiatorId.Id));

            var orderItems = cartItemsToPlace.Select(s => new OrderItem(new ArticleId(s.ArticleId), s.LineText, new Period(s.From, s.To), s.Quantity, s.UnitPrice)).ToList();
            var order = new Order(lessor, lessee, orderItems);

            var orderId = _orderRepository.Add(order);
            command.ResultingOrderId = orderId;

            foreach (var each in cartItemsToPlace)
            {
                cart.RemoveItem(each.CartItemId);
            }

            EventPublisher.Publish(new OrderPlaced(orderId, lessor.LessorId, null));
        }
    }
}