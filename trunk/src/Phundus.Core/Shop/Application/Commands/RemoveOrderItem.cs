namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Collaborators;

    public class RemoveOrderItem : ICommand
    {
        public RemoveOrderItem(InitiatorId initiatorId, OrderId orderId, Guid orderItemId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            InitiatorId = initiatorId;
            OrderId = orderId;
            OrderItemId = orderItemId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public Guid OrderItemId { get; protected set; }
    }

    public class RemoveOrderItemHandler : IHandleCommand<RemoveOrderItem>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICollaboratorService _collaboratorService;

        public RemoveOrderItemHandler(ICollaboratorService collaboratorService, IOrderRepository orderRepository)
        {
            _collaboratorService = collaboratorService;
            _orderRepository = orderRepository;
        }

        [Transaction]
        public void Handle(RemoveOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _collaboratorService.Manager(order.Lessor.LessorId, command.InitiatorId);

            order.RemoveItem(manager, command.OrderItemId);

            _orderRepository.Save(order);
        }
    }
}