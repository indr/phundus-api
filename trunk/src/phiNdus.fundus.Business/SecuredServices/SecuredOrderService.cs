using System;
using System.Collections.Generic;
using System.IO;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using User = phiNdus.fundus.Business.Security.Constraints.User;

namespace phiNdus.fundus.Business.SecuredServices
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

        public IList<OrderDto> GetMyOrders(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService, IList<OrderDto>>(svc => svc.GetMyOrders());
        }

        public void CheckOut(string sessionKey)
        {
            Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService>(svc => svc.CheckOut());
        }

        public OrderDto Reject(string sessionId, int id)
        {
            return Secured.With(Session.FromKey(sessionId))
                .Do<OrderService, OrderDto>(svc => svc.Reject(id));
        }

        public OrderDto Confirm(string sessionId, int id)
        {
            return Secured.With(Session.FromKey(sessionId))
                .Do<OrderService, OrderDto>(svc => svc.Confirm(id));
        }

        public Stream GetPdf(string sessionId, int id)
        {
            return Secured.With(Session.FromKey(sessionId))
                .Do<OrderService, Stream>(svc => svc.GetPdf(id));
        }

        public IList<OrderDto> GetOrders(string sessionKey, OrderStatus status)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<OrderService, IList<OrderDto>>(svc => svc.GetOrders(status));
        }

        #endregion
    }
}