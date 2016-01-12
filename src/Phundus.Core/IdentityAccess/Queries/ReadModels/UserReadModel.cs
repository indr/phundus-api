namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Infrastructure;
    using Integration.IdentityAccess;
    using Users.Model;
    using Users.Repositories;

    public class UserReadModel : IUserQueries
    {
        public IUserRepository UserRepository { get; set; }

        public IUser GetById(int id)
        {
            return GetById(new UserId(id));
        }

        public IUser GetById(UserId userId)
        {
            AssertionConcern.AssertArgumentNotNull(userId, "UserId must be provided.");

            return CreateDto(UserRepository.GetById(userId.Id));
        }

        public IUser FindById(int userId)
        {
            return CreateDto(UserRepository.FindById(userId));
        }

        public IUser FindById(Guid userId)
        {
            return CreateDto(UserRepository.FindByGuid(userId));
        }

        public IUser FindByUsername(string username)
        {
            if (username == null) throw new ArgumentNullException("username");

            return CreateDto(UserRepository.FindByEmailAddress(username));
        }

        public IList<IUser> Query()
        {
            return CreateDtos(UserRepository.FindAll());
        }

        public IUser FindActiveById(Guid userId)
        {
            return CreateDto(UserRepository.FindActiveByGuid(userId));
        }

        public bool IsEmailAddressTaken(string emailAddress)
        {
            var user = UserRepository.FindByEmailAddress(emailAddress);
            return user != null;
        }

        public IUser CreateDto(User subject)
        {
            throw new NotImplementedException();
            //if (subject == null)
            //    return null;
            //return WriteDto(subject);
        }

        public IUser[] CreateDtos(IEnumerable<User> subjects)
        {
            var result = new List<IUser>();
            foreach (var each in subjects)
            {
                result.Add(CreateDto(each));
            }
            return result.ToArray();
        }

        //private static UserDto WriteDto(User subject)
        //{
        //    Guard.Against<ArgumentNullException>(subject == null, "subject");
        //    var result = new UserDto();
        //    result.Id = subject.Id;
        //    result.Guid = subject.Guid;
        //    result.Version = subject.Version;
        //    result.FirstName = subject.FirstName;
        //    result.LastName = subject.LastName;
        //    result.Email = subject.Account.Email;
        //    result.JsNumber = subject.JsNumber;
        //    result.RoleId = (int) subject.Role;
        //    result.RoleName = subject.Role.ToString();
        //    return WriteDtoMembership(subject.Account, result);
        //}

        //private static UserDto WriteDtoMembership(Account subject, UserDto result)
        //{
        //    // TODO: Bei der Registrierung scheint noch ein Bug zu sein, da irgendwie ein Benutzer- aber kein Membership-Datensatz angelegt wird...
        //    if (subject == null)
        //    {
        //        result.IsApproved = false;
        //        result.IsLockedOut = true;
        //        return result;
        //    }
        //    Guard.Against<ArgumentNullException>(subject == null, "subject");
        //    result.Email = subject.Email;
        //    result.CreateDate = subject.CreateDate;
        //    result.IsApproved = subject.IsApproved;
        //    result.IsLockedOut = subject.IsLockedOut;
        //    return result;
        //}
    }
}