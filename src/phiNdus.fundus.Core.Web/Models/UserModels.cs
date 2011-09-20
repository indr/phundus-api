using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using phiNdus.fundus.Core.Business.Dto;
using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Models {
    public class UserModel {

        private int Version { get; set; }

        public int Id { get; private set; }
        
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public static UserModel FromDto(UserDto dto, IEnumerable<RoleDto> roles) {
            
            return new UserModel {
                Id = dto.Id,
                Version = dto.Version,
                Email = dto.Email,
                IsApproved = dto.IsApproved,
                CreateDate = dto.CreateDate,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleId = dto.RoleId,
                RoleName = dto.RoleName,
                Roles = roles.Select(r => new SelectListItem {
                    Value = r.Id.ToString(),                    
                    Text = r.Name,                     
                    Selected = r.Id == dto.RoleId 
                })
            };
        }

        public static UserDto ToDto(UserModel model) {
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