using System;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Mails;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class UserService : BaseService
    {
        private readonly IUserRepository _users;

        public UserService()
        {
            _users = IoC.Resolve<IUserRepository>();
        }

        public UserDto GetUser(string email)
        {
            email = email.ToLowerInvariant();

            using (UnitOfWork.Start())
            {
                User user = _users.FindByEmail(email);
                return UserAssembler.CreateDto(user);
            }
        }

        public UserDto CreateUser(string email, string password)
        {
            email = email.ToLowerInvariant();
            UserDto result;

            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                // Prüfen ob Benutzer bereits exisitiert.
                if (_users.FindByEmail(email) != null)
                    throw new EmailAlreadyTakenException();

                // Neuer Benutzer speichern.
                var user = new User();
                user.Membership.Email = email;
                user.Membership.Password = password;
                _users.Save(user);

                // E-Mail mit Verifikationslink senden
                new UserAccountValidationMail().Send(user);

                result = UserAssembler.CreateDto(user);
                uow.TransactionalFlush();
            }
            return result;
        }

        public void UpdateUser(UserDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = UserAssembler.UpdateDomainObject(subject);
                _users.Update(user);
                uow.TransactionalFlush();
            }
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
                User user = _users.FindByEmail(email);
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