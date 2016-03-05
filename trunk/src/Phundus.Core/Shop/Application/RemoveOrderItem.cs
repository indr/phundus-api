namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

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
        private readonly IUserInRole _userInRole;

        public RemoveOrderItemHandler(IUserInRole userInRole, IOrderRepository orderRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");
            _userInRole = userInRole;
            _orderRepository = orderRepository;
        }

        [Transaction]
        public void Handle(RemoveOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);

            order.RemoveItem(manager, command.OrderItemId);

            _orderRepository.Save(order);
        }
    }
}