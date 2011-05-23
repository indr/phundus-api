using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using User = phiNdus.fundus.Core.Business.Security.Constraints.User;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredUserService : BaseSecuredService, IUserService
    {
        #region IUserService Members

        public UserDto GetUser(string sessionKey, string email)
        {
            return Secured.With(Session.FromKey(sessionKey)
                                && (User.InRole(Role.Administrator)
                                    || User.HasEmail(email))
                ).Do<UserService, UserDto>(svc => svc.GetUser(email));
        }

        public UserDto CreateUser(string sessionKey, string email, string password)
        {
            return Service(sessionKey).CreateUser(email, password);
        }

        public void UpdateUser(string sessionKey, UserDto user)
        {
            // TODO,Inder: Administrator-Rolle oder eigene Benutzer
            Secured.With(Session.FromKey(sessionKey))
                .Do<UserService>(svc => svc.UpdateUser(user));
        }

        public bool DeleteUser(string sessionKey, string email)
        {
            return Service(sessionKey).DeleteUser(email);
        }

        public bool ChangePassword(string sessionKey, string email, string oldPassword, string newPassword)
        {
            return Service(sessionKey).ChangePassword(email, oldPassword, newPassword);
        }

        public string ValidateUser(string email, string password)
        {
            return Service(null).ValidateUser(email, password);
        }

        public string ResetPassword(string sessionKey, string email)
        {
            return Service(sessionKey).ResetPassword(email);
        }

        #endregion

        private static UserService Service(string sessionKey)
        {
            // TODO: SecurityContext-Key
            return new UserService();
        }
    }
}