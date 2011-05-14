using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.Services
{
    public class SecuredUserService : BaseSecuredService, IUserService
    {
        private static UserService Service(string sessionKey)
        {
            // TODO: Session-Key
            return new UserService();
        }

        public UserDto GetUser(string sessionKey, string email)
        {
            return Service(sessionKey).GetUser(email);
        }

        public UserDto CreateUser(string sessionKey, string email, string password)
        {
            return Service(sessionKey).CreateUser(email, password);
        }

        public void UpdateUser(string sessionKey, UserDto user)
        {
            Service(sessionKey).UpdateUser(user);
        }

        public bool DeleteUser(string sessionKey, string email)
        {
            return Service(sessionKey).DeleteUser(email);
        }

        public bool ChangePassword(string sessionKey, string email, string oldPassword, string newPassword)
        {
            return Service(sessionKey).ChangePassword(email, oldPassword, newPassword);
        }

        public bool ValidateUser(string sessionKey, string email, string password)
        {
            return Service(sessionKey).ValidateUser(email, password);
        }

        public string ResetPassword(string sessionKey, string email)
        {
            return Service(sessionKey).ResetPassword(email);
        }
    }
}
