using System;
using System.Net;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Mails;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService()
        {
            _repo = IoC.Resolve<IUserRepository>();
        }
        private IUserRepository _repo;
        private IUserRepository Users { get { return _repo; } }

        public UserDto GetUser(string email)
        {
            email = email.ToLowerInvariant();
            
            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(email);
                return UserAssembler.WriteDto(user);
            }
        }

        public UserDto CreateUser(string email, string password, string passwordQuestion, string passwordAnswer)
        {
            email = email.ToLowerInvariant();
            UserDto result;

            using (var uow = UnitOfWork.Start())
            {
                // Prüfen ob Benutzer bereits exisitiert.
                if (Users.FindByEmail(email) != null)
                    throw new EmailAlreadyTakenException();

                // Neuer Benutzer speichern.
                var user = new User();
                user.Membership.Email = email;
                user.Membership.Password = password;
                user.Membership.PasswordQuestion = passwordQuestion;
                user.Membership.PasswordAnswer = passwordAnswer;
                Users.Save(user);

                // E-Mail mit Verifikationslink senden
                new ValidateUserAccountMail().Send(user);
                
                result = UserAssembler.WriteDto(user);
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
                var user = Users.FindByEmail(email);
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