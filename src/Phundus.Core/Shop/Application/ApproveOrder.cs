namespace Phundus.Shop.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Model;

    public class ApproveOrder
    {
        public InitiatorId InitiatorId { get; set; }
        public OrderId OrderId { get; set; }
    }

    public class ApproveOrderHandler : IHandleCommand<ApproveOrder>
    {
        private readonly IUserInRole _userInRole;
        private readonly IOrderRepository _orderRepository;

        public ApproveOrderHandler(IUserInRole userInRole, IOrderRepository orderRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (orderRepository == null) throw new ArgumentNullException("orderRepository");

            _userInRole = userInRole;
            _orderRepository = orderRepository;
        }

        public void Handle(ApproveOrder command)
        {
            var order = _orderRepository.GetById(command.OrderId);
            var manager = _userInRole.Manager(command.InitiatorId, order.Lessor.LessorId);

            order.Approve(manager);

            _orderRepository.Save(order);
        }
    }
}