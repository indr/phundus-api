namespace phiNdus.fundus.Web.Business.Assembler
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using fundus.Business;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.IdentityAndAccessCtx.DomainModel;
    using Phundus.Core.IdentityAndAccessCtx.Queries;
    using Phundus.Core.IdentityAndAccessCtx.Repositories;
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
            if ((result.Role == null) || (result.Role.Id != subject.RoleId))
                result.Role = ServiceLocator.Current.GetInstance<IRoleRepository>().ById(subject.RoleId);
            return result;
        }
    }
}