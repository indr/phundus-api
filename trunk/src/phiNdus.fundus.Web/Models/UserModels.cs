﻿namespace phiNdus.fundus.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Business.Assembler;
    using Business.Dto;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Core.Entities;
    using Phundus.Core.IdentityAndAccessCtx.DomainModel;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;
    using Phundus.Core.Repositories;


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
            _roles = new List<Role> {Role.Administrator, Role.User}.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(CultureInfo.InvariantCulture),
                    Text = r.Name,
                    Selected = r.Id == RoleId
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