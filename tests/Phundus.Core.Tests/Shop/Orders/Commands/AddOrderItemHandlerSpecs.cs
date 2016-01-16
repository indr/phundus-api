namespace Phundus.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_item_is_handled : order_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private static Period thePeriod;
        private static Order theOrder;
        private static Article theArticle;

        private Establish ctx = () =>
        {
            theLessor = make.Lessor();
            theOrder = fake.an<Order>();
            theOrder.WhenToldTo(x => x.Lessor).Return(theLessor);
            orderRepository.setup(x => x.GetById(theOrder.Id)).Return(theOrder);

            theArticle = make.Article();
            articleRepository.setup(x => x.GetById(theLessor.LessorId, theArticle.ArticleId)).Return(theArticle);

            thePeriod = Period.FromNow(1);

            command = new AddOrderItem(theInitiatorId, theOrder.OrderId, new OrderItemId(), theArticle.ArticleId, thePeriod, 10);
        };

        public It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveChief(theLessor.LessorId.Id, theInitiatorId));

        public It should_tell_order_to_add_item = () =>
            theOrder.WasToldTo(x => x.AddItem(theArticle, thePeriod.FromUtc, thePeriod.ToUtc, 10));
    }
}