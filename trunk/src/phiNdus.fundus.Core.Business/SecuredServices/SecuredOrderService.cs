using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using User = phiNdus.fundus.Core.Business.Security.Constraints.User;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredOrderService : SecuredServiceBase, IOrderService
    {
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
    }
}
