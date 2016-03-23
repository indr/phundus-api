namespace Phundus.Shop.Model.Orders
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Products;

    public class PlaceOrderService
    {
        private readonly IProductsService _productService;

        public PlaceOrderService(IProductsService productService)
        {
            _productService = productService;
        }


        public Order PlaceOrder(Actor actor, OrderId orderId, OrderShortId orderShortId, Lessor lessor, Lessee lessee, ICollection<CartItem> cartItems)
        {
            AssertionConcern.AssertArgumentNotEmpty(cartItems, "Cart items must not be empty.");

            ValidateCartItems(lessor.LessorId, lessee.LesseeId, cartItems);

            return new Order(orderId, orderShortId, lessor, lessee, null);
        }

        private void ValidateCartItems(LessorId lessorId, LesseeId lesseeId, IEnumerable<CartItem> cartItems)
        {
            foreach (var each in cartItems)
            {
                var product = _productService.GetById(lessorId, each.ArticleId, lesseeId);

                if (product.Name != each.LineText)
                    throw new StaleDataException("The cart item text is out of date.");
                if (product.Price != each.UnitPrice)
                    throw new StaleDataException("The cart item price is out of date.");
            }
        }
    }
}