namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Repositories;

    public class UpdateOrderItem
    {
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public InitiatorId InitiatorId { get; set; }

        public int Amount { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public decimal ItemTotal { get; set; }
    }

    public class UpdateOrderItemHandler : IHandleCommand<UpdateOrderItem>
    {
        private readonly IInitiatorService _initiatorService;

        public UpdateOrderItemHandler(IInitiatorService initiatorService)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            _initiatorService = initiatorService;
        }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UpdateOrderItem command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.ChangeAmount(initiator, command.OrderItemId, command.Amount);
            order.ChangeItemPeriod(initiator, command.OrderItemId, command.FromUtc.ToLocalTime().Date.ToUniversalTime(),
                command.ToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());

            order.ChangeItemTotal(initiator, command.OrderItemId, command.ItemTotal);
        }
    }
}