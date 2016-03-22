namespace Phundus.Tests.Shop.Application
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    public class place_order_command_handler_concern : order_command_handler_concern<PlaceOrder, PlaceOrderHandler>
    {
        protected static Cart theCart;
        protected static ICartRepository cartRepository;

        private static OrderId theOrderId;
        private static OrderShortId theOrderShortId;

        private Establish ctx = () =>
        {
            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);

            theCart = new Cart(theInitiatorId);
            cartRepository = depends.on<ICartRepository>();
            cartRepository.setup(x => x.GetByUserGuid(new UserId(theInitiatorId.Id)))
                .Return(theCart);

            command = new PlaceOrder(theInitiatorId, theOrderId, theOrderShortId, theLessor.LessorId);
        };

        protected static CartItemId AddCartItem(LessorId lessorId)
        {
            var anArticle = make.Product(lessorId);
            productsService.setup(x => x.GetById(anArticle.LessorId, anArticle.ArticleId, new LesseeId(theCart.UserId)))
                .Return(anArticle);
            return theCart.AddItem(new CartItemId(), anArticle, Period.FromNow(2), 1);
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

        private It should_add_to_repository_with_two_items = () =>
            orderRepository.WasToldTo(x => x.Add(Arg<Order>.Matches(p =>
                p.Lines.Count == 2
                && p.MutatingEvents.Count == 2)));

        private It should_tell_cart_to_remove_items = () =>
            theCart.Items.ShouldNotContain(c => theCartItemsToRemove.Contains(c.CartItemId));

        private It should_save_cart = () =>
            cartRepository.received(x => x.Save(theCart));
    }
}