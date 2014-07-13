﻿namespace phiNdus.fundus.Web.Business.Assembler
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using fundus.Business;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.Entities;
    using Phundus.Core.Repositories;
    using Phundus.Infrastructure;

    public class UserAssembler
    {
        public UserDto CreateDto(User subject)
        {
            return WriteDto(subject);
        }

        public UserDto[] CreateDtos(ICollection<User> subjects)
        {
            var result = new List<UserDto>();
            foreach (var each in subjects)
            {
                result.Add(CreateDto(each));
            }
            return result.ToArray();
        }

        public static User CreateDomainObject(UserDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new User();
            return WriteDomainObject(subject, result);
        }

        public static User UpdateDomainObject(UserDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = ServiceLocator.Current.GetInstance<IUserRepository>().ById(subject.Id);
            Guard.Against<EntityNotFoundException>(result == null, "User entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            return WriteDomainObject(subject, result);
        }


        private static UserDto WriteDto(User subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            var result = new UserDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            result.JsNumber = subject.JsNumber;
            if (subject.Role != null)
            {
                result.RoleId = subject.Role.Id;
                result.RoleName = subject.Role.Name;
            }
            return WriteDtoMembership(subject.Membership, result);
        }

        private static UserDto WriteDtoMembership(Membership subject, UserDto result)
        {
            // TODO: Bei der Registrierung scheint noch ein Bug zu sein, da irgendwie ein Benutzer- aber kein Membership-Datensatz angelegt wird...
            if (subject == null)
            {
                result.IsApproved = false;
                result.IsLockedOut = true;
                return result;
            }
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            result.Email = subject.Email;
            result.CreateDate = subject.CreateDate;
            result.IsApproved = subject.IsApproved;
            result.IsLockedOut = subject.IsLockedOut;
            return result;
        }

        private static User WriteDomainObject(UserDto subject, User result)
        {
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            if ((result.Role == null) || (result.Role.Id != subject.RoleId))
                result.Role = ServiceLocator.Current.GetInstance<IRoleRepository>().ById(subject.RoleId);
            return result;
        }
    }
}