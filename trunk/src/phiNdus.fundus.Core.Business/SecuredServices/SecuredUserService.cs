using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredUserService : BaseSecuredService, IUserService
    {
        #region IUserService Members

        public UserDto GetUser(string sessionKey, string email)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Call<UserService, UserDto>(svc => svc.GetUser(email));
        }

        public UserDto CreateUser(string sessionKey, string email, string password)
        {
            return Service(sessionKey).CreateUser(email, password);
        }

        public void UpdateUser(string sessionKey, UserDto user)
        {
            Secured.With(Session.FromKey(sessionKey));
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
            // TODO: Session-Key
            return new UserService();
        }
    }
}