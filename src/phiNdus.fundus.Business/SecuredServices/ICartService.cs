﻿using System;
using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Business.SecuredServices
{
    public interface ICartService
    {
        CartDto GetCart(string sessionKey, int? version);
        CartDto AddItem(string sessionKey, CartItemDto item);
        CartDto RemoveItem(string sessionKey, int id, int version);
        CartDto UpdateCart(string sessionKey, CartDto cart);
    }
}