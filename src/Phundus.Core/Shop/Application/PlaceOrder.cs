﻿namespace Phundus.Shop.Application
{
    using System;
    using System.Linq;
    using Authorization;
    using Castle.Transactions;
    using Common;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Model;
    using Model.Collaborators;
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
        private readonly IArticleService _articleService;
        private readonly IAuthorize _authorize;
        private readonly ICartRepository _cartRepository;
        private readonly ICollaboratorService _collaboratorService;
        private readonly ILesseeService _lesseeService;
        private readonly ILessorService _lessorService;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderHandler(IAuthorize authorize, ICollaboratorService collaboratorService,
            ICartRepository cartRepository, IOrderRepository orderRepository,
            ILessorService lessorService, ILesseeService lesseeService, IArticleService articleService)
        {
            _authorize = authorize;
            _collaboratorService = collaboratorService;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _lesseeService = lesseeService;
            _articleService = articleService;
        }

        [Transaction]
        public void Handle(PlaceOrder command)
        {
            var cart = _cartRepository.GetByUserGuid(new UserId(command.InitiatorId.Id));
            AssertionConcern.AssertArgumentFalse(cart.IsEmpty, "Your cart is empty, there is no order to place.");

            var cartItemsToPlace = cart.Items.Where(p => Equals(p.Article.LessorId, command.LessorId)).ToList();
            AssertionConcern.AssertArgumentGreaterThan(cartItemsToPlace.Count, 0,
                String.Format("The cart does not contain items belonging to the lessor {0}.", command.LessorId));

            var initiator = _collaboratorService.Initiator(command.InitiatorId);
            var lessor = _lessorService.GetById(command.LessorId);
            var lessee = _lesseeService.GetById(new LesseeId(command.InitiatorId.Id));

            foreach (var eachCartItem in cartItemsToPlace)
            {
                _articleService.GetById(lessor.LessorId, eachCartItem.ArticleId, lessee.LesseeId);
            }


            var orderLines = new OrderLines(cartItemsToPlace);
            var order = new Order(initiator, command.OrderId, command.OrderShortId, lessor, lessee, orderLines);

            _orderRepository.Add(order);

            foreach (var each in cartItemsToPlace)
            {
                cart.RemoveItem(each.CartItemId);
            }

            EventPublisher.Publish(new OrderPlaced(initiator, order.OrderId, order.OrderShortId,
                lessor, lessee, order.Status, order.OrderTotal, order.Lines.Select(s => new OrderEventLine(
                    s.LineId, s.ArticleId, s.ArticleShortId, s.StoreId, s.Text, s.UnitPricePerWeek, s.Period, s.Quantity,
                    s.LineTotal)).ToList()));
        }
    }
}