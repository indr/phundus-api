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
        public CreateEmptyOrder(InitiatorId initiatorId, LessorId lessorId, LesseeId lesseeId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (lessorId == null) throw new ArgumentNullException("lessorId");
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");
            InitiatorId = initiatorId;
            LessorId = lessorId;
            LesseeId = lesseeId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public LessorId LessorId { get; protected set; }
        public LesseeId LesseeId { get; protected set; }
        
        public int ResultingOrderId { get; set; }
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

            var order = new Order(
                LessorService.GetById(command.LessorId),
                LesseeService.GetById(command.LesseeId));

            OrderRepository.Add(order);

            command.ResultingOrderId = order.ShortOrderId.Id;

            EventPublisher.Publish(new OrderCreated(initiator,
                order.OrderId, order.ShortOrderId, order.Lessor, order.Lessee));
        }
    }
}