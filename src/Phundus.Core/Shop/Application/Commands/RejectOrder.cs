namespace Phundus.Core.Shop.Application.Commands
{
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Queries;

    public class RejectOrder : ICommand
    {
        public int InitiatorId { get; set; }
        public int OrderId { get; set; }
    }

    public class RejectOrderHandler : IHandleCommand<RejectOrder>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(RejectOrder command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            order.Reject(command.InitiatorId);
        }
    }
}