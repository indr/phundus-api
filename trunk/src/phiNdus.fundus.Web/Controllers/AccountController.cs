﻿namespace Phundus.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Exceptions;
    using Core.IdentityAndAccess.Users.Mails;
    using Core.IdentityAndAccess.Users.Model;
    using Core.IdentityAndAccess.Users.Repositories;
    using phiNdus.fundus.Web.Models;
    using phiNdus.fundus.Web.Security;
    using phiNdus.fundus.Web.ViewModels;
    using phiNdus.fundus.Web.ViewModels.Account;

    public class AccountController : ControllerBase
    {
        public CustomMembershipProvider MembershipProvider { get; set; }

        public IUserRepository Users { get; set; }

        public IMembershipQueries MembershipQueries { get; set; }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if ((ModelState.IsValid) && (MembershipProvider.ValidateUser(model.Email, model.Password)))
            {
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                CurrentOrganizationId = null;
                var membership = MembershipQueries.ByUserName(model.Email).FirstOrDefault();
                if (membership != null)
                    CurrentOrganizationId = membership.OrganizationId;

                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", ControllerNames.Home);
            }

            ModelState.AddModelError("", "Benutzername oder Passwort inkorrekt.");
            return View();
        }

        [Authorize]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", ControllerNames.Home);
        }

        [HttpGet]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Eines oder mehrere Felder enthalten ungültige Werte.");
                return View(model);
            }

            try
            {
                MembershipProvider.ResetPassword(model.Email, null);
                
                return View("ResetPasswordDone");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Validation(string id)
        {
            return Validation(new ValidationViewModel {Key = id});
        }

        [HttpPost]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Validation(ValidationViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Key))
            {
                if (MembershipProvider.ValidateValidationKey(model.Key))
                    return View("ValidationDone");
                ModelState.AddModelError("", "Unbekannter oder ungültiger Code.");
            }
            return View(model);
        }

        [HttpGet]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult EmailValidation(string id)
        {
            return EmailValidation(new EmailValidationViewModel {Key = id});
        }

        [HttpPost]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult EmailValidation(EmailValidationViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Key))
            {
                try
                {
                    if (MembershipProvider.ValidateEmailKey(model.Key))
                        return View("EmailValidationDone");
                    ModelState.AddModelError("", "Unbekannter oder ungültiger Code.");
                }
                catch (EmailAlreadyTakenException)
                {
                    ModelState.AddModelError("", "Die E-Mail-Adresse wird bereits verwendet.");
                }
            }
            return View(model);
        }

        // TODO: Profile ist neues Property des Base-Controllers von MVC 4
        public new ActionResult Profile()
        {
            return View();
        }


        [Transaction]
        public virtual ActionResult ChangeEmail()
        {
            return View(new ChangeEmailViewModel());
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Eines oder mehrere Felder enthalten ungültige Werte.");
                return View(model);
            }

            try
            {
                try
                {
                    if (MembershipProvider.ChangeEmail(Identity.Name, model.Email))
                        return View("ChangeEmailDone");
                    ModelState.AddModelError("", "Unbekannter Fehler beim Ändern der E-Mail-Adresse.");
                }
                catch (EmailAlreadyTakenException)
                {
                    ModelState.AddModelError("", "Die E-Mail-Adresse wird bereits verwendet.");
                }


                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [Transaction]
        public virtual ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Eines oder mehrere Felder enthalten ungültige Werte.");
                return View(model);
            }

            try
            {
                if (MembershipProvider.ChangePassword(Identity.Name, model.OldPassword, model.NewPassword))
                    return View("ChangePasswordDone");

                ModelState.AddModelError("", "Unbekannter Fehler beim Ändern des Passwortes.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userDto = new UserDto
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Street = model.Street,
                        Postcode = model.Postcode,
                        City = model.City,
                        MobilePhone = model.MobilePhone,
                        JsNumber = model.JsNumber
                    };
                    var password = model.Password;

                    var email = userDto.Email.ToLower(CultureInfo.CurrentCulture).Trim();

                    // Prüfen ob Benutzer bereits exisitiert.
                    if (Users.FindByEmail(email) != null)
                        throw new EmailAlreadyTakenException();

                    // Neuer Benutzer speichern.
                    var user = new User();
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.Street = userDto.Street;
                    user.Postcode = userDto.Postcode;
                    user.City = userDto.City;
                    user.MobileNumber = userDto.MobilePhone;
                    user.JsNumber = userDto.JsNumber;
                    user.Account.Email = email;
                    user.Account.Password = password;
                    user.Role = Role.User;
                    user.Account.GenerateValidationKey();
                    Users.Add(user);

                    // E-Mail mit Verifikationslink senden
                    new UserAccountValidationMail().For(user).Send(user);

                    return View("SignUpDone");
                }
                catch (EmailAlreadyTakenException)
                {
                    ModelState.AddModelError("Email", "Die E-Mail-Adresse wird bereits verwendet.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Ein oder mehrere Felder enthalten ungültige Daten");
            }

            return View(model);
        }
    }
}