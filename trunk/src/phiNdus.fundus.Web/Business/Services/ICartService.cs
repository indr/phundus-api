namespace phiNdus.fundus.Web.Business.Services
{
    using System.Collections.Generic;
    using Phundus.Core.Shop.Queries;

    public interface ICartService
    {
        CartDto GetCart(int? version);
        CartDto AddItem(CartItemDto item);
        CartDto RemoveItem(int id, int version);
        CartDto UpdateCart(CartDto cart);
        OrderDto PlaceOrder();
        ICollection<OrderDto> PlaceOrders();
    }
}