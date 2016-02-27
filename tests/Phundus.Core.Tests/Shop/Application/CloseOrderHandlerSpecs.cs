namespace Phundus.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;
    using Phundus.Tests.Shop;
    using Rhino.Mocks;

    [Subject(typeof (CloseOrderHandler))]
    public class when_close_order_command_is_handled : order_command_handler_concern<CloseOrder, CloseOrderHandler>
    {
        private static Order theOrder;

        public Establish c = () =>
        {
            theOrder = make.Order(theLessor);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            command = new CloseOrder
            {
                InitiatorId = theInitiatorId,
                OrderId = theOrder.OrderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        public It should_ask_order_to_close =
            () => theOrder.WasToldTo(x => x.Close(theInitiator));

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));
    }
}