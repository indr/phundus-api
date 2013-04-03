using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Assembler {
    using piNuts.phundus.Infrastructure.Rhino;

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
