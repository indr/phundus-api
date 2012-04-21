using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using User = phiNdus.fundus.Core.Business.Security.Constraints.User;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredOrderService : SecuredServiceBase, IOrderService, ICartService
    {
        #region ICartService Members

        public OrderDto GetCart(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService, OrderDto>(svc => svc.GetCart());
        }

        public OrderDto AddItem(string sessionKey, OrderItemDto item)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService, OrderDto>(svc => svc.AddToCart(item));
        }

        public void RemoveItem(string sessionId, int orderItemId, int version)
        {
            Secured.With(Session.FromKey(sessionId))
                .Do<OrderService>(svc => svc.RemoveFromCart(orderItemId, version));
        }

        public void UpdateItems(string sessionId, ICollection<OrderItemDto> orderItemDtos)
        {
            Secured.With(Session.FromKey(sessionId))
                .Do<OrderService>(svc => svc.UpdateCartItems(orderItemDtos));
        }

        #endregion

        #region IOrderService Members

        public OrderDto GetOrder(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService, OrderDto>(svc => svc.GetOrder(id));
        }

        public IList<OrderDto> GetPendingOrders(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<OrderService, IList<OrderDto>>(svc => svc.GetPending());
        }

        public IList<OrderDto> GetApprovedOrders(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<OrderService, IList<OrderDto>>(svc => svc.GetApproved());
        }

        public IList<OrderDto> GetRejectedOrders(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<OrderService, IList<OrderDto>>(svc => svc.GetRejected());
        }

        public IList<OrderDto> GetOrders(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<OrderService, IList<OrderDto>>(svc => svc.GetOrders());
        }

        public void CheckOut(string sessionKey)
        {
            Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService>(svc => svc.CheckOut());
        }

        #endregion
    }
}