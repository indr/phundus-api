namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Collaborators;

    public class RejectOrder : ICommand
    {
        public RejectOrder(InitiatorId initiatorId, OrderId orderId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            InitiatorId = initiatorId;
            OrderId = orderId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
    }

    public class RejectOrderHandler : IHandleCommand<RejectOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICollaboratorService _collaboratorService;

        public RejectOrderHandler(ICollaboratorService collaboratorService, IOrderRepository orderRepository)
        {            
            _collaboratorService = collaboratorService;
            _orderRepository = orderRepository;
        }

        [Transaction]
        public void Handle(RejectOrder command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _collaboratorService.Manager(order.Lessor.LessorId, command.InitiatorId);

            order.Reject(manager);

            _orderRepository.Save(order);
        }
    }
}