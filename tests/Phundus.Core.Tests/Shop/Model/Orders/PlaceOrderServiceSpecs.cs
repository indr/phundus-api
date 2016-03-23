namespace Phundus.Tests.Shop.Model.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using Common;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Orders;
    using Phundus.Shop.Model.Products;
    using Phundus.Shop.Orders.Model;

    public class place_order_service_concern : Observes<PlaceOrderService>
    {
        protected static shop_factory make;

        protected static IAvailabilityService availabilityService;
        protected static IProductsService productService;

        protected static Actor theActor;
        protected static OrderId theOrderId;
        protected static OrderShortId theOrderShortId;
        protected static Lessor theLessor;
        protected static Lessee theLessee;
        protected static ICollection<CartItem> theCartItems; 

        private Establish ctx = () =>
        {
            make = new shop_factory(fake);

            theActor = make.Manager().ToActor();
            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);
            theLessor = make.Lessor();
            theLessee = make.Lessee();
            theCartItems = new Collection<CartItem>();

            productService = depends.on<IProductsService>();
            availabilityService = depends.on<IAvailabilityService>();
        };

        protected static CartItem addCartItem(CartItem cartItem)
        {
            theCartItems.Add(cartItem);
            return cartItem;
        }
    }

    [Subject(typeof (PlaceOrderService))]
    public class when_placing_order : place_order_service_concern
    {
        private static Order result;

        private Establish ctx = () =>
        {
            var product = make.Product();
            productService.setup(x => x.GetById(theLessor.LessorId, product.ArticleId, theLessee.LesseeId))
                .Return(product);
            addCartItem(new CartItem(new CartItemId(), product));
        };

        private Because of = () =>
            result = sut.PlaceOrder(theActor, theOrderId, theOrderShortId, theLessor, theLessee, theCartItems);

        private It should_return_order_with_mutating_event_order_created = () =>
            result.MutatingEvents.ShouldContain(c => c.GetType() == typeof (OrderCreated));

        private It should_return_order_with_mutating_event_order_placed = () =>
            result.MutatingEvents.ShouldContain(c => c.GetType() == typeof (OrderPlaced));
    }

    [Subject(typeof (PlaceOrderService))]
    public class when_trying_to_place_with_empty_items : place_order_service_concern
    {
        private Because of = () => spec.catch_exception(() =>
            sut.PlaceOrder(theActor, theOrderId, theOrderShortId, theLessor, theLessee, new CartItem[0]));

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldEqual("Cart items must not be empty.");

        private It should_throw_invalid_operation_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<InvalidOperationException>();
    }

    [Subject(typeof (PlaceOrderService))]
    public class when_trying_to_place_with_stale_unit_price : place_order_service_concern
    {
        private Establish ctx = () =>
        {
            var product = make.Product();
            var cartItem = addCartItem(make.CartItem(product));
            productService.setup(x => x.GetById(theLessor.LessorId, cartItem.ArticleId, theLessee.LesseeId))
                .Return(make.Product(name: product.Name, price: product.Price + 1.0m));
        };

        private Because of = () => spec.catch_exception(() =>
            sut.PlaceOrder(theActor, theOrderId, theOrderShortId, theLessor, theLessee, theCartItems));

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldEqual("The cart item price is out of date.");

        private It should_throw_stale_data_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<StaleDataException>();
    }


    [Subject(typeof (PlaceOrderService))]
    public class when_trying_to_place_with_stale_text : place_order_service_concern
    {
        private Establish ctx = () =>
        {
            var product = make.Product();
            var cartItem = addCartItem(make.CartItem(product));
            productService.setup(x => x.GetById(theLessor.LessorId, cartItem.ArticleId, theLessee.LesseeId))
                .Return(make.Product(name: "New name"));
        };

        private Because of = () => spec.catch_exception(() =>
            sut.PlaceOrder(theActor, theOrderId, theOrderShortId, theLessor, theLessee, theCartItems));

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldEqual("The cart item text is out of date.");

        private It should_throw_stale_data_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<StaleDataException>();
    }


    [Subject(typeof (PlaceOrderService))]
    public class when_trying_to_place_when_product_is_not_present : place_order_service_concern
    {
        private Establish ctx = () =>
        {
            var cartItem = addCartItem(make.CartItem());
            productService.setup(x => x.GetById(theLessor.LessorId, cartItem.ArticleId, theLessee.LesseeId))
                .Throw(new NotFoundException("Product service exception."));
        };

        private Because of = () => spec.catch_exception(() =>
            sut.PlaceOrder(theActor, theOrderId, theOrderShortId, theLessor, theLessee, theCartItems));

        private It should_throw_exception_message = () =>
            spec.exception_thrown.Message.ShouldMatch("Product service exception.");

        private It should_throw_not_found_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<NotFoundException>();
    }
}