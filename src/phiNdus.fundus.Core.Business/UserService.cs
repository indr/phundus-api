using System;
using Castle.Windsor;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business
{
    public class UserService : BaseService, IUserService
    {
        public UserDto GetUser(string email)
        {
            User user;
            using (UnitOfWork.Start())
            {
                var repo = new UserRepository();
                user = repo.FindByEmail(email);
            }
            var assembler = new UserAssembler();
            return assembler.WriteDto(user);
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