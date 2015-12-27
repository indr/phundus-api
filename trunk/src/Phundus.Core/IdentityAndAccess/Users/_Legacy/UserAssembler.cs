namespace Phundus.Core.IdentityAndAccess.Users._Legacy
{
    using System;
    using Infrastructure;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using Queries;
    using Repositories;

    public class UserAssembler
    {
        public static User UpdateDomainObject(UserDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = ServiceLocator.Current.GetInstance<IUserRepository>().GetById(subject.Id);
            Guard.Against<NotFoundException>(result == null, "User entity not found");
            Guard.Against<DtoOutOfDateException>(result.Version != subject.Version, "Dto is out of date");

            return WriteDomainObject(subject, result);
        }

        private static User WriteDomainObject(UserDto subject, User result)
        {
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            result.Role = (Role) subject.RoleId;
            return result;
        }
    }
}