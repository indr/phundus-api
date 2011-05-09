using System;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business
{
    public class UserService : IUserService
    {
        public UserDto GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public UserDto CreateUser(string email, string password, string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserDto user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string email)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public string ResetPassword(string email)
        {
            throw new NotImplementedException();
        }
    }
}