namespace Phundus.Core.Shop.Orders.Commands
{
    using Cqrs;
    using Ddd;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;

    public class CloseOrder
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

            MemberInRole.ActiveChief(order.OrganizationId, command.InitiatorId);

            order.Close();
        }
    }
}