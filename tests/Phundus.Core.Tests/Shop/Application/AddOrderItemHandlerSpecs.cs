namespace Phundus.Tests.Shop.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Products;

    [Subject(typeof (AddOrderItemHandler))]
    public class when_add_order_command_item_is_handled :
        order_command_handler_concern<AddOrderItem, AddOrderItemHandler>
    {
        private static Period thePeriod;
        private static Order theOrder;
        private static Article theArticle;
        private static OrderLineId theOrderItemId;

        private Establish ctx = () =>
        {
            theOrder = make.Order(theLessor, theLessee);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            theArticle = make.Product();
            productsService.setup(x => x.GetById(theLessor.LessorId, theArticle.ArticleId, theLessee.LesseeId))
                .Return(theArticle);

            thePeriod = Period.FromNow(1);
            theOrderItemId = new OrderLineId();

            command = new AddOrderItem(theInitiatorId, theOrder.OrderId, theOrderItemId, theArticle.ArticleId,
                thePeriod, 10, 1.23m);
        };

        public It should_add_item_to_order = () =>
            theOrder.received(x =>
                x.AddItem(theManager, theOrderItemId, theArticle, thePeriod, 10, 1.23m));

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));
    }
}