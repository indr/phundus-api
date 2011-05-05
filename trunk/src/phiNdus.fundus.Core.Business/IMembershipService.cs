using System;

namespace phiNdus.fundus.Core.Business
{
    public interface IMembershipService
    {
        UserDto CreateUser(Guid sessionKey, string email);
        UserDto GetUser(Guid sessionKey, string name, string password);
        UserDto UpdateUser(Guid sessionKey, UserDto user);
        UserDto ChangePassword(Guid sessionKey, long userId);
        void DeleteUser(Guid sessionKey, long userId);
        bool ValidateUser(Guid sessionKey, string username, string password);
    }
}
