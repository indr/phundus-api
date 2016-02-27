namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Repositories;

    public class RemoveOrderItem
    {
        public OrderId OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public InitiatorId InitiatorId { get; set; }
    }

    public class RemoveOrderItemHandler : IHandleCommand<RemoveOrderItem>
    {
        private readonly IInitiatorService _initiatorService;

        public RemoveOrderItemHandler(IInitiatorService initiatorService)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            _initiatorService = initiatorService;
        }

        public IOrderRepository _orderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RemoveOrderItem command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);

            var order = _orderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.RemoveItem(initiator, command.OrderItemId);

            _orderRepository.Save(order);
        }
    }
}