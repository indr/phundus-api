using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Models
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    public class UserModel : ModelBase
    {
        private IEnumerable<SelectListItem> _roles;

        public UserModel()
        {
            Load(UserService.GetUser(SessionId));
        }
        
        public UserModel(int id)
        {
            Load(UserService.GetUser(SessionId, id));
        }

    
        public int Id { get; private set; }
        public int Version { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }

        [DisplayName("Vorname")]
        public string FirstName { get; set; }

        [DisplayName("Name")]
        public string LastName { get; set; }

        public int RoleId { get; set; }

        [DisplayName("Rolle")]
        public string RoleName { get; set; }

        public string DisplayName { get { return FirstName + " " + LastName; }}

        public IEnumerable<SelectListItem> Roles
        {
            get
            {
                if (_roles == null)
                    GetRoles();
                return _roles;
            }
        }

        private static IUserService UserService
        {
            get { return GlobalContainer.Resolve<IUserService>(); }
        }

        private static IRoleService RoleService
        {
            get { return GlobalContainer.Resolve<IRoleService>(); }
        }

        private void GetRoles()
        {
            var roleDtos = RoleService.GetRoles(SessionId);
            _roles = roleDtos.Select(r => new SelectListItem
                                              {
                                                  Value = r.Id.ToString(),
                                                  Text = r.Name,
                                                  Selected = r.Id == RoleId
                                              });
        }


        private void Load(UserDto subject)
        {
            Id = subject.Id;
            Version = subject.Version;
            Email = subject.Email;
            IsApproved = subject.IsApproved;
            CreateDate = subject.CreateDate;
            FirstName = subject.FirstName;
            LastName = subject.LastName;
            RoleId = subject.RoleId;
            RoleName = subject.RoleName;
        }

        private UserDto Save()
        {
            return new UserDto
                       {
                           Id = Id,
                           Version = Version,
                           Email = Email,
                           IsApproved = IsApproved,
                           CreateDate = CreateDate,
                           FirstName = FirstName,
                           LastName = LastName,
                           RoleId = RoleId,
                           RoleName = RoleName
                       };
        }

        public void Update()
        {
            var subject = Save();
            UserService.UpdateUser(SessionId, subject);
        }
    }
}