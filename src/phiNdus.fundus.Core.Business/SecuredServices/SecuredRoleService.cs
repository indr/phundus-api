using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.SecuredServices {
    public class SecuredRoleService : BaseSecuredService, IRoleService {

        public IEnumerable<RoleDto> GetRoles(string sessionKey) {
            return Secured.With(null)
                .Do<RoleService, IEnumerable<RoleDto>>(s => s.GetRoles());
        }
    }
}
