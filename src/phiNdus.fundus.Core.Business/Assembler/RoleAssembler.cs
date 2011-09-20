using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler {
    public static class RoleAssembler {

        public static RoleDto CreateDto(Role subject) {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            return new RoleDto {
                Id = subject.Id,
                Name = subject.Name
            };
        }
    }
}
