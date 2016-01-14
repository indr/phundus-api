namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Model;
    using Repositories;
    using Shop.Services;

    public class PlaceOrder : ICommand
    {
        public PlaceOrder(UserId initiatorId, LessorId lessorId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            InitiatorId = initiatorId;
            LessorId = lessorId;
        }

        public UserId InitiatorId { get; protected set; }
        public LessorId LessorId { get; protected set; }
    }

    public class PlaceOrderHandler : IHandleCommand<PlaceOrder>
    {
        private readonly ILesseeService _lesseeService;
        private readonly ILessorService _lessorService;
        private readonly IOrderRepository _orderRepository;

        public PlaceOrderHandler(IOrderRepository orderRepository, ILessorService lessorService,
            ILesseeService lesseeService)
        {
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            if (lessorService == null) throw new ArgumentNullException("lessorService");
            if (lesseeService == null) throw new ArgumentNullException("lesseeService");
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _lesseeService = lesseeService;
        }

        public void Handle(PlaceOrder command)
        {
            var lessor = _lessorService.GetById(command.LessorId);
            var lessee = _lesseeService.GetById(new LesseeId(command.InitiatorId.Id));

            var order = new Order(lessor, lessee);

            _orderRepository.Add(order);
        }
    }
}