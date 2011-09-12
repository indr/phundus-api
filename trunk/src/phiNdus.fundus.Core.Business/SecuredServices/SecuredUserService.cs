using System;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;
using User = phiNdus.fundus.Core.Business.Security.Constraints.User;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredUserService : BaseSecuredService, IUserService
    {
        #region IUserService Members

        public UserDto GetUser(string sessionKey, string email)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.HasEmail(email) || User.InRole(Role.Administrator))
                .Do<UserService, UserDto>(svc => svc.GetUser(email));
        }

        public UserDto GetUser(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.Is(id) || User.InRole(Role.Administrator))
                .Do<UserService, UserDto>(svc => svc.GetUser(id));
        }

        public UserDto CreateUser(string sessionKey, string email, string password, string firstName, string lastName)
        {
            return Secured.With(null)
                .Do<UserService, UserDto>(svc => svc.CreateUser(email, password, firstName, lastName));
        }

        public void UpdateUser(string sessionKey, UserDto user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Secured.With(Session.FromKey(sessionKey))
                .And(User.Is(user.Id) || User.InRole(Role.Administrator))
                .Do<UserService>(svc => svc.UpdateUser(user));
        }

        public bool DeleteUser(string sessionKey, string email)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .And(User.HasEmail(email) || User.InRole(Role.Administrator))
                .Do<UserService, bool>(svc => svc.DeleteUser(email));
        }

        public bool ChangePassword(string sessionKey, string email, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string sessionId, string email, string password)
        {
            return Secured.With(null)
                .Do<UserService, bool>(svc => svc.ValidateUser(sessionId, email, password));
        }

        public string ResetPassword(string sessionKey, string email)
        {
            throw new NotImplementedException();
        }

        public bool ValidateValidationKey(string key)
        {
            return Secured.With(null)
                .Do<UserService, bool>(svc => svc.ValidateValidationKey(key));
        }

        public UserDto[] GetUsers(string sessionKey)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<UserService, UserDto[]>(svc => svc.GetUsers());
        }

        #endregion
    }
}