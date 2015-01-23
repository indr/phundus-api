namespace Phundus.Core.Shop.Application.Commands
{
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Ordering;
    using IdentityAndAccess.Queries;

    public class CloseOrder : ICommand
    {
        public int OrderId { get; set; }
        public int InitiatorId { get; set; }
    }

    public class CloseOrderHandler : IHandleCommand<CloseOrder>
    {
        public IOrderRepository OrderRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(CloseOrder command)
        {
            var order = OrderRepository.GetById(command.OrderId);

            MemberInRole.ActiveChief(order.Organization.Id, command.InitiatorId);

            order.Close(command.InitiatorId);
        }
    }
}