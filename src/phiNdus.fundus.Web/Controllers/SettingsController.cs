﻿using System;
using System.Web.Mvc;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Web.Helpers;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private static string MasterView { get { return @"Index"; } }

        private static class Views
        {
            public static string General { get { return @"General"; } }
            public static string MailGeneral { get { return @"MailGeneral"; } }
            public static string MailTemplates { get { return @"MailTemplates"; } }
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return General();
        }

        [HttpGet]
        public ActionResult General()
        {
            return View(Views.General, MasterView);
        }

        [HttpGet]
        public ActionResult MailGeneral()
        {
            ModelState.Clear();
            var model = new SmtpSettingsViewModel();
            model.Load();
            return View(Views.MailGeneral, MasterView, model);
        }

        [HttpPost]
        public ActionResult MailGeneral(SmtpSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = UnitOfWork.Start())
                {
                    model.SaveChanges();
                    uow.TransactionalFlush();
                }
            }

            return MailGeneral();
        }

        [HttpGet]
        public ActionResult MailTemplates()
        {
            ModelState.Clear();
            var model = new MailTemplateSettingsViewModel();
            model.Load();
            return View(Views.MailTemplates, MasterView, model);
        }

        [HttpPost]
        public ActionResult MailTemplates(MailTemplateSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                    using (var uow = UnitOfWork.Start())
                    {
                        model.SaveChanges();
                        uow.TransactionalFlush();
                    }
            }

            return MailTemplates();
        }

        [HttpPost]
        public ActionResult SendTestEmail(SendTestEmailViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ModelStateException(ModelState);

            var gateway = new MailGateway(model.TestHost, model.TestUserName, model.TestPassword, model.TestFrom);
            gateway.Send(model.TestTo, "[fundus] Test E-Mail", model.TestBodyTemplate);

            return null;
        }
    }
}