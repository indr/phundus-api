using System;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using User = phiNdus.fundus.Core.Business.Security.Constraints.User;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredPropertyService : SecuredServiceBase, IPropertyService
    {
        #region IPropertyService Members

        public PropertyDto[] GetProperties(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<PropertyService, PropertyDto[]>(svc => svc.GetProperties());
        }

        public void UpdateProperty(string sessionKey, PropertyDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService>(svc => svc.UpdateProperty(subject));
        }

        public PropertyDto GetProperty(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<PropertyService, PropertyDto>(svc => svc.GetProperty(id));
        }

        public int CreateProperty(string sessionKey, PropertyDto subject)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService, int>(svc => svc.CreateProperty(subject));
        }

        #endregion
    }
}