namespace Phundus.Shop.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

    public class RemoveOrderItem
    {
        public OrderId OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public InitiatorId InitiatorId { get; set; }
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

        public void Handle(RemoveOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);

            order.RemoveItem(manager, command.OrderItemId);

            _orderRepository.Save(order);
        }
    }
}