namespace phiNdus.fundus.Business.SecuredServices
{
    using System.Collections.Generic;
    using Castle.Transactions;
    using phiNdus.fundus.Business.Dto;
    using phiNdus.fundus.Business.Security;
    using phiNdus.fundus.Business.Security.Constraints;
    using phiNdus.fundus.Business.Services;

    public class SecuredRoleService : SecuredServiceBase, IRoleService
    {
        [Transaction]
        public IEnumerable<RoleDto> GetRoles(string sessionKey)
        {
            return Secured.With(null)
                          .Do<RoleService, IEnumerable<RoleDto>>(s => s.GetRoles());
        }

        [Transaction]
        public virtual string[] GetRolesForUser(string sessionKey)
        {
            try
            {
                return Secured.With(Session.FromKey(sessionKey))
                              .Do<RoleService, string[]>(s => s.GetRolesForUser());
            }
            catch (InvalidSessionKeyException)
            {
                return Unsecured.Do<RoleService, string[]>(svc => svc.GetRolesForUser());
            }
        }
    }
}