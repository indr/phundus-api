namespace phiNdus.fundus.Web.Business.Assembler
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using fundus.Business;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.IdentityAndAccess.Queries;
    using Phundus.Core.IdentityAndAccess.Users.Model;
    using Phundus.Core.IdentityAndAccess.Users.Repositories;
    using Phundus.Infrastructure;

    public class UserAssembler
    {
        

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

        private static User WriteDomainObject(UserDto subject, User result)
        {
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            result.Role = (Role) subject.RoleId;
            return result;
        }
    }
}