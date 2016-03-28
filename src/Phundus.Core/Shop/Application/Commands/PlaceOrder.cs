namespace Phundus.Shop.Application
{
    using System;
    using System.Linq;
    using Castle.Transactions;
    using Common;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Inventory.Application;
    using Model;
    using Model.Orders;
    using Model.Products;
    using Orders.Model;

    public class PlaceOrder : ICommand
    {
        public PlaceOrder(InitiatorId initiatorId, OrderId orderId, OrderShortId orderShortId, LessorId lessorId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            InitiatorId = initiatorId;
            OrderId = orderId;
            OrderShortId = orderShortId;
            LessorId = lessorId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public OrderShortId OrderShortId { get; protected set; }
        public LessorId LessorId { get; protected set; }
    }

    public class PlaceOrderHandler : IHandleCommand<PlaceOrder>
    {
        private readonly IProductsService _productsService;
        private readonly IAvailabilityQueryService _availabilityQueryService;
        private readonly ICartRepository _cartRepository;
        private readonly ILesseeService _lesseeService;
        private readonly ILessorService _lessorService;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderHandler(ICartRepository cartRepository, IOrderRepository orderRepository,
            ILessorService lessorService, ILesseeService lesseeService, IProductsService productsService,
            IAvailabilityQueryService availabilityQueryService)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _lesseeService = lesseeService;
            _productsService = productsService;
            _availabilityQueryService = availabilityQueryService;
        }

        [Transaction]
        public void Handle(PlaceOrder command)
        {
            var cart = _cartRepository.GetByUserGuid(new UserId(command.InitiatorId.Id));
            AssertionConcern.AssertArgumentFalse(cart.IsEmpty, "Your cart is empty, there is no order to place.");

            var items = cart.TakeItems(command.LessorId);
            AssertionConcern.AssertArgumentGreaterThan(items.Count, 0,
                String.Format("The cart does not contain items belonging to the lessor {0}.", command.LessorId));

            var lessor = _lessorService.GetById(command.LessorId);
            var lessee = _lesseeService.GetById(new LesseeId(command.InitiatorId.Id));

            var order = new PlaceOrderService(_productsService, _availabilityQueryService)
                .PlaceOrder(command.OrderId, command.OrderShortId, lessor, lessee, items);

            _orderRepository.Add(order);
            _cartRepository.Save(cart);
        }
    }
}