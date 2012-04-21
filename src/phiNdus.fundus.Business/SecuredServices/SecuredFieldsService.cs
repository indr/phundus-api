using System;
using System.Collections.Generic;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using User = phiNdus.fundus.Business.Security.Constraints.User;

namespace phiNdus.fundus.Business.SecuredServices
{
    public class SecuredFieldsService : SecuredServiceBase, IFieldsService
    {
        #region IFieldsService Members

        public IList<FieldDefinitionDto> GetProperties(string sessionKey)
        {
            return Unsecured.Do<PropertyService, IList<FieldDefinitionDto>>(svc => svc.GetProperties());
        }

        public void UpdateField(string sessionKey, FieldDefinitionDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService>(svc => svc.UpdateProperty(subject));
        }

        public FieldDefinitionDto GetField(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<PropertyService, FieldDefinitionDto>(svc => svc.GetProperty(id));
        }

        public int CreateField(string sessionKey, FieldDefinitionDto subject)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService, int>(svc => svc.CreateProperty(subject));
        }

        public void DeleteField(string sessionKey, FieldDefinitionDto subject)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService>(svc => svc.DeleteProperty(subject));
        }

        public void UpdateFields(string sessionKey, IList<FieldDefinitionDto> subjects)
        {
            Secured.With(Session.FromKey(sessionKey))
                .And(User.InRole(Role.Administrator))
                .Do<PropertyService>(svc => svc.UpdateFields(subjects));
        }

        #endregion
    }
}