using System;
using System.Globalization;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Mails;
using phiNdus.fundus.Core.Domain;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class UserService : BaseService
    {
        private static IUserRepository Users { get { return IoC.Resolve<IUserRepository>(); }}
        private static IRoleRepository Roles { get { return IoC.Resolve<IRoleRepository>(); }}

        public virtual UserDto GetUser(string email)
        {
            email = email.ToLower(CultureInfo.CurrentCulture);

            using (UnitOfWork.Start())
            {
                var user = Users.FindByEmail(email);
                if (user == null)
                    return null;
                return UserAssembler.CreateDto(user);
            }
        }

        public virtual UserDto CreateUser(string email, string password)
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
                user.Membership.Email = email;
                user.Membership.Password = password;
                user.Role = Roles.Get(Role.User.Id);
                user.Membership.GenerateValidationKey();
                Users.Save(user);

                // E-Mail mit Verifikationslink senden
                new UserAccountValidationMail().For(user).Send(user);

                result = UserAssembler.CreateDto(user);
                uow.TransactionalFlush();
            }
            return result;
        }

        public virtual void UpdateUser(UserDto subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            using (var uow = UnitOfWork.Start())
            {
                var user = UserAssembler.UpdateDomainObject(subject);
                Users.Update(user);
                uow.TransactionalFlush();
            }
        }

        public virtual bool DeleteUser(string email)
        {
            Guard.Against<ArgumentNullException>(email == null, "email");
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(email);
                Users.Delete(user);
                uow.TransactionalFlush();
            }
            return true;
        }

        public virtual bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public virtual string ValidateUser(string email, string password)
        {
            email = email.ToLower(CultureInfo.CurrentCulture);

            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(email);
                if (user == null)
                    return null;
                try
                {
                    user.Membership.LogOn(password);
                    uow.TransactionalFlush();
                    return user.Membership.SessionKey;
                }
                catch (InvalidPasswordException)
                {
                    return null;
                }
            }
        }

        public virtual string ResetPassword(string email)
        {
            throw new NotImplementedException();
        }
    }
}