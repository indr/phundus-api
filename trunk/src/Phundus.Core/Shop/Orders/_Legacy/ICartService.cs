namespace Phundus.Shop.Orders
{
    using Common.Domain.Model;
    using Queries;

    public interface ICartService
    {
        CartDto GetCartByUserId(int userId);
        CartDto AddItem(int? cartId, int userId, CartItemDto item);
        CartDto RemoveItem(int userId, CartItemGuid itemGuid, int version);
        CartDto UpdateCart(CartDto cart);

        bool PlaceOrders(int userId);
    }
}