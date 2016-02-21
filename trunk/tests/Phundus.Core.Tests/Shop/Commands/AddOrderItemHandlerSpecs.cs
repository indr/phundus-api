namespace Phundus.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_command_item_is_handled :
        order_command_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private static Period thePeriod;
        private static Order theOrder;
        private static Article theArticle;
        private static OrderItemId theOrderItemId;

        private Establish ctx = () =>
        {
            theOrder = make.Order();
            theLessor = theOrder.Lessor;
            theLessee = theOrder.Lessee;
            orderRepository.setup(x => x.GetById(theOrder.ShortOrderId.Id)).Return(theOrder);

            theArticle = make.Article();
            articleService.setup(x => x.GetById(theLessor.LessorId, theArticle.ArticleShortId, theLessee.LesseeId))
                .Return(theArticle);

            thePeriod = Period.FromNow(1);
            theOrderItemId = new OrderItemId();

            command = new AddOrderItem(theInitiatorId, theOrder.ShortOrderId, theOrderItemId, theArticle.ArticleShortId,
                thePeriod,
                10);
        };

        public It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        public It should_tell_order_to_add_item = () =>
            theOrder.WasToldTo(x =>
                x.AddItem(theInitiator, theOrderItemId, theArticle, thePeriod.FromUtc, thePeriod.ToUtc, 10));
    }
}