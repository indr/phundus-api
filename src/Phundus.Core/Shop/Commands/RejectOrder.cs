namespace Phundus.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Queries;
    using Integration.IdentityAccess;
    using Repositories;

    public class RejectOrder
    {
        public InitiatorId InitiatorId { get; set; }
        public int OrderId { get; set; }
    }

    public class RejectOrderHandler : IHandleCommand<RejectOrder>
    {
        private readonly IInitiatorService _initiatorService;

        public RejectOrderHandler(IInitiatorService initiatorService)
        {
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            _initiatorService = initiatorService;
        }

        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RejectOrder command)
        {
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);

            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveManager(order.Lessor.LessorId.Id, command.InitiatorId);

            order.Reject(initiator);
        }
    }
}