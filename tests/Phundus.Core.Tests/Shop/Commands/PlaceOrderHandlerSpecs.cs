namespace Phundus.Tests.Shop.Commands
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Orders.Commands;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;
    using Rhino.Mocks;

    public class place_order_command_handler_concern : order_command_handler_concern<PlaceOrder, PlaceOrderHandler>
    {
        protected const int theResultingOrderId = 123;
        protected static Cart theCart;
        protected static ICartRepository cartRepository;

        private static int _nextArticleId = 1;

        private Establish ctx = () =>
        {
            lesseeService.WhenToldTo(x => x.GetById(new LesseeId(theInitiatorId.Id)))
                .Return(CreateLessee(theInitiatorId.Id));

            orderRepository.setup(x => x.Add(Arg<Order>.Is.NotNull)).Return(theResultingOrderId);

            theCart = new Cart(theInitiatorId, theInitiatorId);
            depends.on<ICartRepository>()
                .WhenToldTo(x => x.GetByUserGuid(new UserId(theInitiatorId.Id)))
                .Return(theCart);

            command = new PlaceOrder(theInitiatorId, theLessor.LessorId);
        };

        protected static CartItemId AddCartItem(LessorId lessorId)
        {
            var anArticle = make.Article(lessorId.Id);
            articleService.setup(x => x.GetById(anArticle.LessorId, anArticle.ArticleShortId, new LesseeId(theCart.UserGuid)))
                .Return(anArticle);
            return theCart.AddItem(anArticle, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1);
        }
    }

    [Subject(typeof (PlaceOrderHandler))]
    public class when_trying_to_place_an_empty_cart : place_order_command_handler_concern
    {
        private Establish ctx = () => catchException = true;

        private It should_not_add_to_repository = () =>
            orderRepository.WasNotToldTo(x => x.Add(Arg<Order>.Is.Anything));

        private It should_not_publish_order_placed = () =>
            NotPublished<OrderPlaced>();

        private It should_throw_exception_with_message = () =>
            caughtException.Message.ShouldEqual("Your cart is empty, there is no order to place.");

        private It should_throw_invalid_operation_exception = () =>
            caughtException.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (PlaceOrderHandler))]
    public class when_trying_to_place_a_cart_with_only_items_from_different_lessor : place_order_command_handler_concern
    {
        private static LessorId theOtherLessorId;

        private Establish ctx = () =>
        {
            catchException = true;
            theOtherLessorId = new LessorId();
            AddCartItem(lessorId: theOtherLessorId);
        };

        private It should_not_add_to_repository = () =>
            orderRepository.WasNotToldTo(x => x.Add(Arg<Order>.Is.Anything));

        private It should_not_publish_order_placed = () =>
            NotPublished<OrderPlaced>();

        private It should_throw_exception_with_message = () =>
            caughtException.Message.ShouldEqual(
                String.Format("The cart does not contain items belonging to the lessor {0}.", theLessor.LessorId));

        private It should_throw_invalid_operation_exception = () =>
            caughtException.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (PlaceOrderHandler))]
    public class when_successfully_placing_an_cart_with_items_from_different_lessors :
        place_order_command_handler_concern
    {
        private static List<CartItemId> theCartItemsToRemove;

        private Establish ctx = () =>
        {
            theCartItemsToRemove = new List<CartItemId>();
            var anOtherLessorId = new LessorId();
            theCartItemsToRemove.Add(AddCartItem(theLessor.LessorId));
            AddCartItem(anOtherLessorId);
            theCartItemsToRemove.Add(AddCartItem(theLessor.LessorId));
        };

        private It should_add_to_repository_with_two_items =
            () => orderRepository.WasToldTo(x => x.Add(Arg<Order>.Matches(p => p.Items.Count == 2)));

        private It should_publish_order_placed = () =>
            Published<OrderPlaced>(p =>
                p.OrderId == theResultingOrderId
                && p.LessorId == theLessor.LessorId.Id);

        private It should_set_resulting_order_id =
            () => command.ResultingOrderId.ShouldEqual(theResultingOrderId);

        private It should_tell_cart_to_remove_items =
            () => theCart.Items.ShouldNotContain(c => theCartItemsToRemove.Contains(c.CartItemId));
    }
}