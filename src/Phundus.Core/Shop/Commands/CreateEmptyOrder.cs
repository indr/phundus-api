namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Model;
    using Repositories;
    using Shop.Services;

    public class CreateEmptyOrder
    {
        public CreateEmptyOrder(InitiatorId initiatorId, OrderId orderId, OrderShortId orderShortId, LessorId lessorId, LesseeId lesseeId)
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
        private readonly IInitiatorService _initiatorService;

        public CreateEmptyOrderHandler(IInitiatorService initiatorService)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            _initiatorService = initiatorService;
        }

        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public ILessorService LessorService { get; set; }

        public ILesseeService LesseeService { get; set; }

        public void Handle(CreateEmptyOrder command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var ownerId = new OwnerId(command.LessorId.Id);
            MemberInRole.ActiveManager(ownerId, command.InitiatorId);

            var order = new Order(initiator,
                command.OrderId, command.OrderShortId,
                LessorService.GetById(command.LessorId),
                LesseeService.GetById(command.LesseeId));

            OrderRepository.Add(order);

            EventPublisher.Publish(new OrderCreated(initiator,
                order.OrderId, order.OrderShortId, order.Lessor, order.Lessee));
        }
    }
}