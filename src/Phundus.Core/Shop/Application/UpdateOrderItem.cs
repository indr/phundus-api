namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

    public class UpdateOrderItem : ICommand
    {
        public UpdateOrderItem(InitiatorId initiatorId, OrderId orderId, Guid orderItemId, Period period, int quantity, decimal lineTotal)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (period == null) throw new ArgumentNullException("period");
            InitiatorId = initiatorId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            Period = period;
            Quantity = quantity;
            LineTotal = lineTotal;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrderId OrderId { get; protected set; }
        public Guid OrderItemId { get; protected set; }
        public Period Period { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal LineTotal { get; protected set; }
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

        [Transaction]
        public void Handle(UpdateOrderItem command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);


            order.ChangeQuantity(manager, command.OrderItemId, command.Quantity);
            order.ChangeItemPeriod(manager, command.OrderItemId, command.Period.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.Period.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());

            order.ChangeItemTotal(manager, command.OrderItemId, command.LineTotal);

            _orderRepository.Save(order);
        }
    }
}