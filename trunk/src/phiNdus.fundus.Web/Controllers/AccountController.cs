using System;
using System.Web.Mvc;
using System.Web.Security;
using phiNdus.fundus.Web.Models;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            FormsService = IoC.Resolve<IFormsService>();
            MembershipService = IoC.Resolve<IMembershipService>();
        }

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
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Benutzername oder Passwort inkorrekt.");
            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
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

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                //MembershipService.CreateUser(model.Email, model.Password, out status);
                MembershipService.CreateUser(model.Email, model.Password, model.FirstName, model.LastName, out status);


                switch (status)
                {
                    case MembershipCreateStatus.Success:
                        return View("SignUpDone");
                        //case MembershipCreateStatus.InvalidUserName:
                        //    break;
                        //case MembershipCreateStatus.InvalidPassword:
                        //    break;
                        //case MembershipCreateStatus.InvalidQuestion:
                        //    break;
                        //case MembershipCreateStatus.InvalidAnswer:
                        //    break;
                        //case MembershipCreateStatus.InvalidEmail:
                        //    break;
                        //case MembershipCreateStatus.DuplicateUserName:
                        //    break;
                    case MembershipCreateStatus.DuplicateEmail:
                        ModelState.AddModelError("", "Die E-Mail-Adresse wird bereits verwendet.");
                        break;
                        //case MembershipCreateStatus.UserRejected:
                        //    break;
                        //case MembershipCreateStatus.InvalidProviderUserKey:
                        //    break;
                        //case MembershipCreateStatus.DuplicateProviderUserKey:
                        //    break;
                        //case MembershipCreateStatus.ProviderError:
                        //    break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                ModelState.AddModelError("", "Ein oder mehrere Felder enthalten ungültige Daten");
            }

            // Nicht erfolgreich
            return View();
        }
    }
}