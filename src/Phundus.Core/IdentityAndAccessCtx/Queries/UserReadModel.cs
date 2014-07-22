namespace Phundus.Core.IdentityAndAccessCtx.Queries
{
    using System;
    using System.Collections.Generic;
    using DomainModel;
    using Infrastructure;
    using Repositories;

    public class UserReadModel : IUserQueries
    {
        public IUserRepository UserRepository { get; set; }

        public UserDto ById(int id)
        {
            return CreateDto(UserRepository.FindById(id));
        }

        public IList<UserDto> All()
        {
            return CreateDtos(UserRepository.FindAll());
        }

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

    }
}