﻿using System;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserDto GetUser(string email)
        {
            email = email.ToLowerInvariant();
            
            using (UnitOfWork.Start())
            {
                var repo = IoC.Resolve<IUserRepository>();
                var user = repo.FindByEmail(email);
                var assembler = new UserAssembler();
                return assembler.WriteDto(user);
            }
        }

        public UserDto CreateUser(string email, string password, string passwordQuestion, string passwordAnswer)
        {
            email = email.ToLowerInvariant();
            UserDto result;

            using (var uow = UnitOfWork.Start())
            {
                var repo = IoC.Resolve<IUserRepository>();
                if (repo.FindByEmail(email) != null)
                    throw new EmailAlreadyTakenException();

                var user = new User();
                user.Membership.Email = email;
                user.Membership.Password = password;
                user.Membership.PasswordQuestion = passwordQuestion;
                user.Membership.PasswordAnswer = passwordAnswer;
                repo.Save(user);
                
                var assembler = new UserAssembler();
                result = assembler.WriteDto(user);
                
                uow.TransactionalFlush();
            }
            return result;
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
            email = email.ToLowerInvariant();

            using (UnitOfWork.Start())
            {
                var repo = IoC.Resolve<IUserRepository>();
                var user = repo.FindByEmail(email);
                if (user == null)
                    return false;
                // TODO,Inder: Password encryption
                return user.Membership.Password == password;
            }
        }

        public string ResetPassword(string email)
        {
            throw new NotImplementedException();
        }
    }
}