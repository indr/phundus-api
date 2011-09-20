using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.Models {
    public class UserDetailModel {

        private int Version { get; set; }

        public int Id { get; private set; }
        
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        //public IList<RolesDto

        public static UserDetailModel FromDto(UserDto dto) {
            return new UserDetailModel {
                Id = dto.Id,
                Version = dto.Version,
                Email = dto.Email,
                IsApproved = dto.IsApproved,
                CreateDate = dto.CreateDate,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleId = dto.RoleId,
                RoleName = dto.RoleName
            };
        }

        public static UserDto ToDto(UserDetailModel model) {
            return new UserDto {
                Id = model.Id,
                Version = model.Version,
                Email = model.Email,
                IsApproved = model.IsApproved,
                CreateDate = model.CreateDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RoleId = model.RoleId,
                RoleName = model.RoleName
            };
        }
    }
}