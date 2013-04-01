using System;
using System.Web.Mvc;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Web.ViewModels;
using RazorEngine;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private static string MasterView
        {
            get { return @"Index"; }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return General();
        }

        [HttpGet]
        public ActionResult General()
        {
            ModelState.Clear();
            var model = new GeneralSettingsViewModel();
            model.Load();
            return View(Views.General, MasterView, model);
        }

        [HttpPost]
        public ActionResult General(GeneralSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var uow = UnitOfWork.Start())
                {
                    model.SaveChanges();
                    uow.TransactionalFlush();
                }
                return General();
            }

            return View(Views.General, MasterView, model);
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
                return MailGeneral();
            }
            return View(Views.MailGeneral, MasterView, model);
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
                return MailTemplates();
            }

            return View(Views.MailTemplates, MasterView, model);
        }

        [HttpPost]
        public ActionResult SendTestEmail(SendTestEmailViewModel model)
        {
            var body = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    body = Razor.Parse(model.TestBodyTemplate);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("TestBodyTemplate", ex.GetType().Name + ": " + ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = 400;
                HttpContext.Response.Clear();
                return PartialView(@"~\Views\Settings\EditorTemplates\SendTestEmailViewModel.cshtml", model);
            }


            var gateway = new MailGateway();
            gateway.Send(model.TestTo, "[fundus] Test E-Mail", body);

            return null;
        }

        #region Nested type: Views

        private static class Views
        {
            public static string General
            {
                get { return @"General"; }
            }

            public static string MailGeneral
            {
                get { return @"MailGeneral"; }
            }

            public static string MailTemplates
            {
                get { return @"MailTemplates"; }
            }
        }

        #endregion
    }
}