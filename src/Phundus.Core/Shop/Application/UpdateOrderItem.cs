namespace Phundus.Shop.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

    public class UpdateOrderItem
    {
        public OrderId OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public InitiatorId InitiatorId { get; set; }

        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Quantity { get; set; }
        public decimal ItemTotal { get; set; }
    }

    public class UpdateOrderItemHandler : IHandleCommand<UpdateOrderItem>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserInRole _userInRole;

        public UpdateOrderItemHandler(IUserInRole userInRole, IOrderRepository orderRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");

            _userInRole = userInRole;
            _orderRepository = orderRepository;
        }

        public void Handle(UpdateOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);


            order.ChangeQuantity(manager, command.OrderItemId, command.Quantity);
            order.ChangeItemPeriod(manager, command.OrderItemId, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());

            order.ChangeItemTotal(manager, command.OrderItemId, command.ItemTotal);

            _orderRepository.Save(order);
        }
    }
}