using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;
using phiNdus.fundus.Business.Services;

namespace phiNdus.fundus.Business.SecuredServices {
    public class SecuredRoleService : SecuredServiceBase, IRoleService {

        public IEnumerable<RoleDto> GetRoles(string sessionKey) {
            return Secured.With(null)
                .Do<RoleService, IEnumerable<RoleDto>>(s => s.GetRoles());
        }

        public string[] GetRolesForUser(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<RoleService, string[]>(s => s.GetRolesForUser());
        }
    }
}
