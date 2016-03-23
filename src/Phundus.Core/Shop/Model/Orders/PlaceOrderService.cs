namespace Phundus.Shop.Model.Orders
{
    using System.Collections.Generic;
    using Common;

    public class PlaceOrderService
    {
        public Order PlaceOrder(ICollection<CartItem> cartItems)
        {
            AssertionConcern.AssertArgumentNotEmpty(cartItems, "Cart items must not be empty.");

            return null;
        }
    }
}