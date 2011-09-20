using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.Services {
    internal class RoleService : BaseService {

        public virtual IEnumerable<RoleDto> GetRoles() {
            return new List<RoleDto> {
                RoleAssembler.CreateDto(Role.Administrator),
                RoleAssembler.CreateDto(Role.User),
            };
        }
    }
}
