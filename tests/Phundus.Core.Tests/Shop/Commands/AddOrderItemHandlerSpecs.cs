﻿namespace Phundus.Tests.Shop.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Orders.Commands;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Commands;

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
            theOrder = make.Order();
            theLessor = theOrder.Lessor;
            theLessee = theOrder.Lessee;
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            theArticle = make.Article();
            articleService.setup(x => x.GetById(theLessor.LessorId, theArticle.ArticleId, theLessee.LesseeId))
                .Return(theArticle);

            thePeriod = Period.FromNow(1);
            theOrderItemId = new OrderLineId();

            command = new AddOrderItem(theInitiatorId, theOrder.OrderId, theOrderItemId, theArticle.ArticleId,
                thePeriod, 10);
        };

        public It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));

        public It should_tell_order_to_add_item = () =>
            theOrder.WasToldTo(x =>
                x.AddItem(theInitiator, theOrderItemId, theArticle, thePeriod, 10));
    }
}