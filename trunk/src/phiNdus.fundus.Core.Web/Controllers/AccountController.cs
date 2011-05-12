using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Web.Security;
using phiNdus.fundus.Core.Web.Models;

namespace phiNdus.fundus.Core.Web.Controllers {

    public class AccountController : Controller {
        public IMembershipService MembershipService { get; set; }
        public IFormsService FormsService { get; set; }    

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            // Todo,chris: per Castle laden?
            if (FormsService == null) FormsService = new FormsAuthenticationService();
            //if (MembershipService == null) MembershipService = new AccountMembershipService();

            base.Initialize(requestContext);
        }

        public ActionResult LogOn() {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl) {
            if (ModelState.IsValid) {
                if (MembershipService.ValidateUser(model.EMail, model.Password)) {

                    FormsService.SignIn(model.EMail, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl)) {
                        return Redirect(returnUrl);
                    } else {
                        return RedirectToAction("Index", "Home");
                    }
                }
            } else {
                ModelState.AddModelError("", "Benutzername oder Passwort inkorrekt");
            }

            // Nicht erfolgreich
            return View();
        }

        [Authorize]
        public ActionResult LogOff() {

            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
