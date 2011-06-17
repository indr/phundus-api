﻿using System;
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
            // FIXME,chris: Wieder aktivieren
            //if (MembershipService == null) MembershipService = new AccountMembershipService();

            base.Initialize(requestContext);
        }

        public ActionResult LogOn() {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl) {
            if (ModelState.IsValid) {
                if (MembershipService.ValidateUser(model.Email, model.Password)) {

                    FormsService.SignIn(model.Email, model.RememberMe);
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

        public ActionResult SignUp() {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model) {
            if (ModelState.IsValid) {
                // TODO,chris: Benutzer erstellen

                return RedirectToAction("Index", "Home");
            } else {
                ModelState.AddModelError("", "Ein oder mehrere Felder enthalten ungültige Daten");
            }

            // Nicht erfolgreich
            return View();
        }

    }
}
