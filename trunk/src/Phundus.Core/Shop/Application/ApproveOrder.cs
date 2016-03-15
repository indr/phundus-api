namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Collaborators;

    public class ApproveOrder : ICommand
    {
        public ApproveOrder(InitiatorId initiatorId, OrderId orderId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            InitiatorId = initiatorId;
            OrderId = orderId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
    }

    public class ApproveOrderHandler : IHandleCommand<ApproveOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICollaboratorService _collaboratorService;

        public ApproveOrderHandler(ICollaboratorService collaboratorService, IOrderRepository orderRepository)
        {
            _collaboratorService = collaboratorService;
            _orderRepository = orderRepository;
        }

        [Transaction]
        public void Handle(ApproveOrder command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _collaboratorService.Manager(command.InitiatorId, order.Lessor.LessorId);

            order.Approve(manager);

            _orderRepository.Save(order);
        }
    }
}