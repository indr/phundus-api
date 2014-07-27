﻿namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Users.Model;
    using Users.Repositories;

    public class UserReadModel : IUserQueries
    {
        public IUserRepository UserRepository { get; set; }

        public UserDto ById(int id)
        {
            return CreateDto(UserRepository.FindById(id));
        }

        public UserDto ByEmail(string email)
        {
            return CreateDto(UserRepository.FindByEmail(email));
        }

        public IList<UserDto> All()
        {
            return CreateDtos(UserRepository.FindAll());
        }

        public UserDto CreateDto(User subject)
        {
            if (subject == null)
                return null;
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

        private static UserDto WriteDto(User subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            var result = new UserDto();
            result.Id = subject.Id;
            result.Version = subject.Version;
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            result.Email = subject.Account.Email;
            result.JsNumber = subject.JsNumber;
            result.RoleId = (int) subject.Role;
            result.RoleName = subject.Role.ToString();
            return WriteDtoMembership(subject.Account, result);
        }

        private static UserDto WriteDtoMembership(Account subject, UserDto result)
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

    }
}