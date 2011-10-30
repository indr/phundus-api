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

        public FieldDefinitionDto[] GetProperties(string sessionKey)
        {
            return Unsecured.Do<PropertyService, FieldDefinitionDto[]>(svc => svc.GetProperties());
        }

        public void UpdateProperty(string sessionKey, FieldDefinitionDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService>(svc => svc.UpdateProperty(subject));
        }

        public FieldDefinitionDto GetProperty(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<PropertyService, FieldDefinitionDto>(svc => svc.GetProperty(id));
        }

        public int CreateProperty(string sessionKey, FieldDefinitionDto subject)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService, int>(svc => svc.CreateProperty(subject));
        }

        public void DeleteProperty(string sessionKey, FieldDefinitionDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService>(svc => svc.DeleteProperty(subject));
        }

        #endregion
    }
}