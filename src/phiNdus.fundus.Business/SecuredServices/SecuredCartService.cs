using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;
using phiNdus.fundus.Business.Services;

namespace phiNdus.fundus.Business.SecuredServices
{
    public class SecuredCartService : SecuredServiceBase, ICartService
    {
        public CartDto GetCart(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<CartService, CartDto>(svc => svc.GetCart());
        }
    }
}
