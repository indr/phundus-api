namespace Phundus.Tests.Shop.Model.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Orders;
    using Phundus.Shop.Orders.Model;

    public class place_order_service_concern : Observes<PlaceOrderService>
    {
        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new PlaceOrderService());
    }

    [Subject(typeof (PlaceOrderService))]
    public class when_trying_to_place_with_empty_items : place_order_service_concern
    {
        private Because of = () => spec.catch_exception(() =>
            sut.PlaceOrder(new CartItem[0]));

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldEqual("Cart items must not be empty.");

        private It should_throw_invalid_operation_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (PlaceOrderService))]
    public class when_placing_order : place_order_service_concern
    {
        private static ICollection<CartItem> cartItems = new Collection<CartItem>();
        private static Order result;

        private Establish ctx = () =>
            cartItems.Add(new CartItem(new CartItemId()));

        private Because of = () =>
            result = sut.PlaceOrder(cartItems);

        private It should_return_order_with_mutating_event_order_created;// = () =>
            //result.MutatingEvents.ShouldContain(c => c.GetType() == typeof (OrderCreated));

        private It should_return_order_with_mutating_event_order_placed;// = () =>
            //result.MutatingEvents.ShouldContain(c => c.GetType() == typeof (OrderPlaced));
    }
}