using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services {
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

            if (SecurityContext.SecuritySession == null)
                return new string[0];

            var role = SecurityContext.SecuritySession.User.Role.Name;
            if (role == "Admin")
                return new[] {"User", "Admin"};
            return new[] {role};
        }
    }
}
