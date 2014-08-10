namespace Phundus.Core.Shop.Orders
{
    using System.Collections.Generic;
    using Queries;

    public interface ICartService
    {
        CartDto GetCartByUserId(int userId);
        CartDto AddItem(int? cartId, int userId, CartItemDto item);
        CartDto RemoveItem(int id, int version);
        CartDto UpdateCart(CartDto cart);
        OrderDto PlaceOrder();
        ICollection<OrderDto> PlaceOrders();
    }
}