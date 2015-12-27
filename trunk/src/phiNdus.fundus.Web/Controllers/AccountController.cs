namespace Phundus.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Commands;
    using Core.IdentityAndAccess.Users.Exceptions;
    using Core.IdentityAndAccess.Users.Mails;
    using Core.IdentityAndAccess.Users.Repositories;
    using phiNdus.fundus.Web.Models;
    using phiNdus.fundus.Web.Security;
    using phiNdus.fundus.Web.ViewModels;
    using phiNdus.fundus.Web.ViewModels.Account;

    public class AccountController : ControllerBase
    {
        public CustomMembershipProvider MembershipProvider { get; set; }

        public IUserRepository Users { get; set; }

        public IOrganizationQueries OrganizationQueries { get; set; }

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
            var model = new SignUpModel();
            model.Organizations = GetOrganizationSelectListItems(model);
            return View(model);
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
                    var command = new RegisterUser(
                        model.Email, model.Password, model.FirstName,
                        model.LastName, model.Street, model.Postcode,
                        model.City, model.MobilePhone);
                    Dispatcher.Dispatch(command);


                    var user = Users.FindById(command.UserId);
                    // E-Mail mit Verifikationslink senden
                    new UserAccountValidationMail().For(user).Send(user);

                    Guid organizationId;                    
                    if ((Guid.TryParse(model.OrganizationId, out organizationId)))
                    {
                        Dispatcher.Dispatch(new ApplyForMembership
                        {
                            ApplicantId = user.Id,
                            OrganizationId = organizationId
                        });
                    }

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
            model.Organizations = GetOrganizationSelectListItems(model);
            return View(model);
        }

        private List<SelectListItem> GetOrganizationSelectListItems(SignUpModel model)
        {
            var result =
                OrganizationQueries.AllNonFree()
                    .Select(
                        s =>
                            new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Guid.ToString(),
                                Selected = s.Guid.ToString() == model.OrganizationId
                            })
                    .ToList();
            result.Insert(0, new SelectListItem());
            return result;
        }
    }
}