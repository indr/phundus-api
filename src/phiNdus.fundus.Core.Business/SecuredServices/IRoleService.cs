using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.SecuredServices {
    public interface IRoleService {

        /// <summary>
        /// Liefert alle im System definierten Rollen.
        /// </summary>
        IEnumerable<RoleDto> GetRoles(string sessionKey);
    }
}
