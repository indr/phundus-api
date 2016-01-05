namespace phiNdus.fundus.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Phundus.Common.Domain.Model;
    using Phundus.Core.IdentityAndAccess.Queries;

    public class UserModel
    {
        private IEnumerable<SelectListItem> _roles;

        public UserModel(UserDto user)
        {
            Load(user);
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

        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }

        public IEnumerable<SelectListItem> Roles
        {
            get
            {
                if (_roles == null)
                    GetRoles();
                return _roles;
            }
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


        private void GetRoles()
        {
            _roles = new List<UserRole> {UserRole.Admin, UserRole.User}.Select(r => new SelectListItem
            {
                Value = ((int) r).ToString(CultureInfo.InvariantCulture),
                Text = r.ToString(),
                Selected = (int) r == RoleId
            });
        }
    }
}