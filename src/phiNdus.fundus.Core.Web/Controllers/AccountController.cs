using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using phiNdus.fundus.Core.Web.Models;
using phiNdus.fundus.Core.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController() {
            this.FormsService = IoC.Resolve<IFormsService>();
            this.MembershipService = IoC.Resolve<IMembershipService>();
        }

        private IMembershipService MembershipService { get; set; }
        private IFormsService FormsService { get; set; }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    // Todo,chris: per Castle laden?
        //    //if (FormsService == null)
        //    //    FormsService = new FormsAuthenticationService();            

        //    //if (MembershipService == null)
        //    //    MembershipService = new MembershipService();

        //    base.Initialize(requestContext);
        //}

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
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                
            }
            else
            {
                ModelState.AddModelError("", "Benutzername oder Passwort inkorrekt.");
            }

            // Nicht erfolgreich
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
                else
                {
                    ModelState.AddModelError("", "Unbekannter oder ungültiger Code.");
                }
            }
            return View(model);
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