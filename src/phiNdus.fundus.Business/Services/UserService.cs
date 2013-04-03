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
    using System.Configuration;
    using Domain.Infrastructure;
    using piNuts.phundus.Infrastructure;

    public class UserService : BaseService
    {
        private static IUserRepository Users
        {
            get { return GlobalContainer.Resolve<IUserRepository>(); }
        }

        private static IOrganizationRepository Organizations
        {
            get { return GlobalContainer.Resolve<IOrganizationRepository>(); }
        }

        private static IRoleRepository Roles
        {
            get { return GlobalContainer.Resolve<IRoleRepository>(); }
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
            Guard.Against<ArgumentNullException>(email == null, "email");
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(email);
                user.Membership.ChangePassword(oldPassword, newPassword);
                uow.TransactionalFlush();
            }
            return true;
        }


        public bool ChangeEmail(string email, string newEmail)
        {
            Guard.Against<ArgumentNullException>(email == null, "email");
            Guard.Against<ArgumentNullException>(newEmail == null, "newEmail");
            email = email.ToLower(CultureInfo.CurrentCulture).Trim();
            newEmail = newEmail.ToLower(CultureInfo.CurrentCulture).Trim();

            using (var uow = UnitOfWork.Start())
            {
                // Prüfen ob Benutzer bereits exisitiert.
                if (Users.FindByEmail(newEmail) != null)
                    throw new EmailAlreadyTakenException();

                var user = Users.FindByEmail(email);
                user.Membership.RequestedEmail = newEmail;
                user.Membership.GenerateValidationKey();
                Users.Update(user);
                new UserChangeEmailValidationMail().For(user).Send(user);
                uow.TransactionalFlush();
            }
            return true;
        }

        public virtual bool ValidateUser(string sessionId, string email, string password)
        {
            email = email.ToLower(CultureInfo.CurrentCulture).Trim();

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
            Guard.Against<ArgumentNullException>(email == null, "email");
            email = email.ToLower(CultureInfo.CurrentCulture).Trim();

            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByEmail(email);
                if (user == null)
                    throw new Exception("Die E-Mail-Adresse konnte nicht gefunden werden.");
                var password = user.Membership.ResetPassword();
                Users.Update(user);
                new UserResetPasswordMail().For(user, password).Send(user);
                uow.TransactionalFlush();
                return password;
            }
        }

        public UserDto CreateUser(UserDto userDto, string password, int? organizationId)
        {
            var email = userDto.Email.ToLower(CultureInfo.CurrentCulture).Trim();
            UserDto result;

            using (var uow = UnitOfWork.Start())
            {
                // Prüfen ob Benutzer bereits exisitiert.
                if (Users.FindByEmail(email) != null)
                    throw new EmailAlreadyTakenException();

                Organization organization = null;
                if (organizationId.HasValue)
                {
                    organization = Organizations.FindById(organizationId.Value);
                    if (organization == null)
                        throw new Exception(String.Format("Die Organization mit der Id {0} ist nicht vorhanden.",
                                                          organizationId));
                }

                // Neuer Benutzer speichern.
                var user = new User();
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Street = userDto.Street;
                user.Postcode = userDto.Postcode;
                user.City = userDto.City;
                user.MobileNumber = userDto.MobilePhone;
                user.JsNumber = userDto.JsNumber;
                user.Membership.Email = email;
                user.Membership.Password = password;
                user.Role = Roles.Get(Role.User.Id);
                user.Membership.GenerateValidationKey();
                if (organization != null)
                    user.Join(organization);
                Users.Save(user);

                // E-Mail mit Verifikationslink senden
                new UserAccountValidationMail().For(user).Send(user);

                result = new UserAssembler().CreateDto(user);
                uow.TransactionalFlush();
            }
            return result;
        }

        public virtual UserDto CreateUser(string email, string password, string firstName, string lastName, int jsNumber,
                                          int? organizationId)
        {
            email = email.ToLower(CultureInfo.CurrentCulture).Trim();
            UserDto result;

            using (var uow = UnitOfWork.Start())
            {
                // Prüfen ob Benutzer bereits exisitiert.
                if (Users.FindByEmail(email) != null)
                    throw new EmailAlreadyTakenException();

                Organization organization = null;
                if (organizationId.HasValue)
                {
                    organization = Organizations.FindById(organizationId.Value);
                    if (organization == null)
                        throw new Exception(String.Format("Die Organization mit der Id {0} ist nicht vorhanden.",
                                                          organizationId));
                }

                // Neuer Benutzer speichern.
                var user = new User();
                user.FirstName = firstName;
                user.LastName = lastName;
                user.JsNumber = jsNumber;
                user.Membership.Email = email;
                user.Membership.Password = password;
                user.Role = Roles.Get(Role.User.Id);
                user.Membership.GenerateValidationKey();
                if (organization != null)
                    user.Join(organization);
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
                    Users.Update(user);

                new UserAccountCreatedMail().For(user).Send(Config.FeedbackRecipients);

                uow.TransactionalFlush();
            }
            return result;
        }

        public virtual bool ValidateEmailKey(string key)
        {
            var result = false;
            using (var uow = UnitOfWork.Start())
            {
                var user = Users.FindByValidationKey(key);
                if (user == null)
                    return false;

                // Prüfen ob Benutzer bereits exisitiert.
                if (Users.FindByEmail(user.Membership.RequestedEmail) != null)
                    throw new EmailAlreadyTakenException();

                result = user.Membership.ValidateEmailKey(key);
                if (result)
                    Users.Update(user);

                //new UserAccountCreatedMail().For(user).Send(Settings.Common.AdminEmailAddress);

                uow.TransactionalFlush();
            }
            return result;
        }

        
    }
}