namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Collaborators;

    public class CreateEmptyOrder : ICommand
    {
        public CreateEmptyOrder(InitiatorId initiatorId, OrderId orderId, OrderShortId orderShortId, LessorId lessorId,
            LesseeId lesseeId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (orderShortId == null) throw new ArgumentNullException("orderShortId");
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");
            InitiatorId = initiatorId;
            OrderId = orderId;
            OrderShortId = orderShortId;
            LessorId = lessorId;
            LesseeId = lesseeId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public OrderShortId OrderShortId { get; protected set; }
        public LessorId LessorId { get; protected set; }
        public LesseeId LesseeId { get; protected set; }
    }

    public class CreateEmptyOrderHandler : IHandleCommand<CreateEmptyOrder>
    {
        private readonly ILesseeService _lesseeService;
        private readonly ILessorService _lessorService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICollaboratorService _collaboratorService;

        public CreateEmptyOrderHandler(ICollaboratorService collaboratorService, IOrderRepository orderRepository,
            ILessorService lessorService, ILesseeService lesseeService)
        {            
            _collaboratorService = collaboratorService;
            _orderRepository = orderRepository;
            _lessorService = lessorService;
            _lesseeService = lesseeService;
        }

        [Transaction]
        public void Handle(CreateEmptyOrder command)
        {
            var manager = _collaboratorService.Manager(command.InitiatorId, command.LessorId);

            var order = new Order(manager,
                command.OrderId, command.OrderShortId,
                _lessorService.GetById(command.LessorId),
                _lesseeService.GetById(command.LesseeId));

            _orderRepository.Add(order);
        }
    }
}