using System;
using System.Web.Mvc;
using System.Web.Security;
using phiNdus.fundus.Business;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Web.Models;
using phiNdus.fundus.Web.ViewModels;
using phiNdus.fundus.Web.ViewModels.Account;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;

    public class AccountController : ControllerBase
    {
        public AccountController()
        {
            FormsService = GlobalContainer.Resolve<IFormsService>();
            MembershipService = GlobalContainer.Resolve<IMembershipService>();
        }

        public IOrganizationRepository Organizations { get; set; }
        public IUserService UserService { get; set; }

        private IFormsService FormsService { get; set; }
        private IMembershipService MembershipService { get; set; }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if ((ModelState.IsValid) && (MembershipService.ValidateUser(model.Email, model.Password)))
            {
                FormsService.SignIn(model.Email, model.RememberMe);
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", ControllerNames.Home);
            }

            ModelState.AddModelError("", "Benutzername oder Passwort inkorrekt.");
            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", ControllerNames.Home);
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Eines oder mehrere Felder enthalten ungültige Werte.");
                return View(model);
            }

            try
            {
                if (MembershipService.ResetPassword(model.Email))
                    return View("ResetPasswordDone");

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
        public ActionResult Validation(string id)
        {
            return Validation(new ValidationViewModel {Key = id});
        }

        [HttpPost]
        public ActionResult Validation(ValidationViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Key))
            {
                if (MembershipService.ValidateValidationKey(model.Key))
                    return View("ValidationDone");
                ModelState.AddModelError("", "Unbekannter oder ungültiger Code.");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EmailValidation(string id)
        {
            return EmailValidation(new EmailValidationViewModel {Key = id});
        }

        [HttpPost]
        public ActionResult EmailValidation(EmailValidationViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Key))
            {
                try
                {
                    if (MembershipService.ValidateEmailKey(model.Key))
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


        public ActionResult ChangeEmail()
        {
            return View(new ChangeEmailViewModel());
        }

        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
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
                    if (MembershipService.ChangeEmailAddress(User.Identity.Name, model.Email))
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

        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Eines oder mehrere Felder enthalten ungültige Werte.");
                return View(model);
            }

            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
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
        public ActionResult SignUp()
        {
            using (UnitOfWork.Start())
            {
                var model = new SignUpModel
                                {
                                    Organizations = Organizations.FindAll()
                                };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {


                try
                {
                    UserService.CreateUser(HttpContext.Session.SessionID,
                                           new UserDto
                                               {
                                                   Email = model.Email,
                                                   FirstName = model.FirstName,
                                                   LastName = model.LastName,
                                                   Street = model.Street,
                                                   Postcode = model.Postcode,
                                                   City = model.City,
                                                   MobilePhone = model.MobilePhone,
                                                   JsNumber = model.JsNumber
                                               },
                                           model.Password,
                                           model.OrganizationId);
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

            // Nicht erfolgreich
            using (UnitOfWork.Start())
                model.Organizations = Organizations.FindAll();
            return View(model);
        }
    }
}