using System;
using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Business.SecuredServices
{
    public interface ICartService
    {
        CartDto GetCart(string sessionKey);
        CartDto AddItem(string sessionKey, CartItemDto item);
        //void RemoveItem(string sessionId, int orderItemId, int version);
        //void UpdateItems(string sessionId, ICollection<OrderItemDto> orderItemDtos);
    }
}