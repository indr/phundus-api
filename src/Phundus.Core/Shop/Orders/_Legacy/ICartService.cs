namespace Phundus.Shop.Orders
{
    using Queries;

    public interface ICartService
    {
        CartDto GetCartByUserId(int userId);
        CartDto AddItem(int? cartId, int userId, CartItemDto item);
    }
}