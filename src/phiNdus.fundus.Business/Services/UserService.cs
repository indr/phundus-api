using System;
using System.Globalization;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Mails;
using phiNdus.fundus.Domain;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Services
{
    public class UserService : BaseService
    {
        private static IUserRepository Users
        {
            get { return IoC.Resolve<IUserRepository>(); }
        }

        private static IRoleRepository Roles
        {
            get { return IoC.Resolve<IRoleRepository>(); }
        }

        public virtual UserDto GetUser(string email)
        {
            email = email.ToLower(CultureInfo.CurrentCulture);

            using (UnitOfWork.Start())
            {
                User user = Users.FindByEmail(email);
                if (user == null)
                    return null;
                return new UserAssembler().CreateDto(user);
            }
        }

        public UserDto GetUser()
        {
            using (UnitOfWork.Start())
            {
                return new UserAssembler().CreateDto(SecurityContext.SecuritySession.User);
            }
        }

        public virtual UserDto GetUser(int id)
        {
            using (UnitOfWork.Start())
            {
                User user = Users.Get(id);
                if (user == null)
                    return null;
                return new UserAssembler().CreateDto(user);
            }
        }

        public UserDto[] GetUsers()
        {
            using (var uow = UnitOfWork.Start())
            {
                return new UserAssembler().CreateDtos(Users.FindAll());
            }
        }

        public virtual void UpdateUser(UserDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = UserAssembler.UpdateDomainObject(subject);
                Users.Update(user);
                uow.TransactionalFlush();
            }
        }

        public virtual bool DeleteUser(string email)
        {
            Guard.Against<ArgumentNullException>(email == null, "email");
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = Users.FindByEmail(email);
                Users.Delete(user);
                uow.TransactionalFlush();
            }
            return true;
        }

        public virtual bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public virtual bool ValidateUser(string sessionId, string email, string password)
        {
            email = email.ToLower(CultureInfo.CurrentCulture);

            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = Users.FindByEmail(email);
                if (user == null)
                    return false;
                try
                {
                    user.Membership.LogOn(sessionId, password);
                    uow.TransactionalFlush();
                    return true;
                }
                catch (InvalidPasswordException)
                {
                    return false;
                }
            }
        }

        public virtual string ResetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public virtual UserDto CreateUser(string email, string password, string firstName, string lastName, int jsNumber)
        {
            email = email.ToLower(CultureInfo.CurrentCulture);
            UserDto result;

            using (var uow = UnitOfWork.Start())
            {
                // Prüfen ob Benutzer bereits exisitiert.
                if (Users.FindByEmail(email) != null)
                    throw new EmailAlreadyTakenException();

                // Neuer Benutzer speichern.
                var user = new User();
                user.FirstName = firstName;
                user.LastName = lastName;
                user.JsNumber = jsNumber;
                user.Membership.Email = email;
                user.Membership.Password = password;
                user.Role = Roles.Get(Role.User.Id);
                user.Membership.GenerateValidationKey();
                Users.Save(user);

                // E-Mail mit Verifikationslink senden
                new UserAccountValidationMail().For(user).Send(user);

                result = new UserAssembler().CreateDto(user);
                uow.TransactionalFlush();
            }
            return result;
        }

        public virtual bool ValidateValidationKey(string key)
        {
            var result = false;
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByValidationKey(key);
                if (user == null)
                    return false;

                result = user.Membership.ValidateValidationKey(key);
                if (result)
                    Users.Save(user);

                new UserAccountCreatedMail().For(user).Send(Settings.Common.AdminEmailAddress);

                uow.TransactionalFlush();
            }
            return result;
        }

    }
}