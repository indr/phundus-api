namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

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
        private readonly IUserInRole _userInRole;

        public RejectOrderHandler(IUserInRole userInRole, IOrderRepository orderRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            _userInRole = userInRole;
            _orderRepository = orderRepository;
        }

        [Transaction]
        public void Handle(RejectOrder command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);

            order.Reject(manager);

            _orderRepository.Save(order);
        }
    }
}