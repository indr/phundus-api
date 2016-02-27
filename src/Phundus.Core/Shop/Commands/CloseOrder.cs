namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Projections;
    using Integration.IdentityAccess;
    using Shop.Model;

    public class CloseOrder
    {
        public InitiatorId InitiatorId { get; set; }
        public OrderId OrderId { get; set; }
    }

    public class CloseOrderHandler : IHandleCommand<CloseOrder>
    {
        private readonly IInitiatorService _initiatorService;

        public CloseOrderHandler(IInitiatorService initiatorService)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            _initiatorService = initiatorService;
        }

        public IOrderRepository _orderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(CloseOrder command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);

            var order = _orderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.Close(initiator);

            _orderRepository.Save(order);
        }
    }
}