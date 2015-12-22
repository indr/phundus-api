﻿namespace phiNdus.fundus.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Core.IdentityAndAccess.Queries;
    using Phundus.Core.IdentityAndAccess.Users.Model;
    using Phundus.Core.IdentityAndAccess.Users.Repositories;
    using Phundus.Core.IdentityAndAccess.Users._Legacy;


    public class UserModel : ModelBase
    {
        IEnumerable<SelectListItem> _roles;

        public UserModel(UserDto user)
        {
            Load(user);
        }

        void Load(UserDto subject)
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

       

        void GetRoles()
        {
            _roles = new List<Role> {Role.Admin, Role.User}.Select(r => new SelectListItem
                {
                    Value = ((int)r).ToString(CultureInfo.InvariantCulture),
                    Text = r.ToString(),
                    Selected = (int)r == RoleId
                });
        }


        

        UserDto Save()
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
            var user = UserAssembler.UpdateDomainObject(subject);
            ServiceLocator.Current.GetInstance<IUserRepository>().Update(user);
        }
    }
}