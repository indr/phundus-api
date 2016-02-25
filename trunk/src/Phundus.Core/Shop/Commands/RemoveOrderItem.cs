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
        public int OrderId { get; set; }
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

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RemoveOrderItem command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);

            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.RemoveItem(initiator, command.OrderItemId);
        }
    }
}