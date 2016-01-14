namespace Phundus.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Queries;
    using Model;
    using Repositories;
    using Shop.Services;

    public class CreateEmptyOrder
    {
        public CurrentUserId InitiatorId { get; set; }
        public LessorId LessorId { get; set; }
        public LesseeId LesseeId { get; set; }
        
        public int ResultingOrderId { get; set; }
    }

    public class CreateEmptyOrderHandler : IHandleCommand<CreateEmptyOrder>
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository Repository { get; set; }

        public ILessorService LessorService { get; set; }

        public ILesseeService LesseeService { get; set; }

        public void Handle(CreateEmptyOrder command)
        {
            var ownerId = new OwnerId(command.LessorId.Id);
            MemberInRole.ActiveChief(ownerId, command.InitiatorId);

            var order = new Order(
                LessorService.GetById(command.LessorId),
                LesseeService.GetById(command.LesseeId));

            var orderId = Repository.Add(order);

            command.ResultingOrderId = orderId;

            EventPublisher.Publish(new OrderCreated(orderId));
        }
    }
}