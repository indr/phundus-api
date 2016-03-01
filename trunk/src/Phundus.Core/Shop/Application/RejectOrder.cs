namespace Phundus.Shop.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;

    public class RejectOrder
    {
        public InitiatorId InitiatorId { get; set; }
        public OrderId OrderId { get; set; }
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

        public void Handle(RejectOrder command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);

            order.Reject(manager);

            _orderRepository.Save(order);
        }
    }
}