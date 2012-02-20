using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services {
    internal class RoleService : BaseService {

        public virtual IEnumerable<RoleDto> GetRoles() {
            return new List<RoleDto> {
                RoleAssembler.CreateDto(Role.Administrator),
                RoleAssembler.CreateDto(Role.User),
            };
        }

        public string[] GetRolesForUser()
        {
            if (SecurityContext == null)
                return new string[0];

            var role = SecurityContext.SecuritySession.User.Role.Name;
            if (role == "Admin")
                return new[] {"User", "Admin"};
            return new[] {role};
        }
    }
}
