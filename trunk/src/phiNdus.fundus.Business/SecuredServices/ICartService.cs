using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Business.SecuredServices
{
    public interface ICartService
    {
        OrderDto GetCart(string sessionKey);
        OrderDto AddItem(string sessionKey, OrderItemDto orderItemDto);
        void RemoveItem(string sessionId, int orderItemId, int version);
        void UpdateItems(string sessionId, ICollection<OrderItemDto> orderItemDtos);
    }
}