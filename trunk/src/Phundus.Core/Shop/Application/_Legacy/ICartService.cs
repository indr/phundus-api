namespace Phundus.Core.Shop.Application
{
    using System.Collections.Generic;
    using Queries;

    public interface ICartService
    {
        CartDto GetCartByUserId(int userId);
        CartDto AddItem(int? cartId, int userId, CartItemDto item);
        CartDto RemoveItem(int userId, int itemId, int version);
        CartDto UpdateCart(CartDto cart);
        
        ICollection<LegacyOrderDto> PlaceOrders(int userId);
    }
}