using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;

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

        #endregion
    }
}