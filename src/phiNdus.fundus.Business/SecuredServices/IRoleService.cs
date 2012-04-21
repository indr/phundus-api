using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Business.Dto;

namespace phiNdus.fundus.Business.SecuredServices {
    public interface IRoleService {

        /// <summary>
        /// Liefert alle im System definierten Rollen.
        /// </summary>
        IEnumerable<RoleDto> GetRoles(string sessionKey);

        /// <summary>
        /// Liefert die Rollen, in welchen der angegeben Benutzer ist.
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        string[] GetRolesForUser(string sessionKey);
    }
}
