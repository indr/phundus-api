using System;
using System.Web.Mvc;
using phiNdus.fundus.Web.ViewModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        //
        // GET: /Settings/

        public ActionResult Index()
        {
            return MailTemplates();
        }

        public ActionResult MailTemplates()
        {
            ModelState.Clear();
            var model = new MailTemplateSettingsViewModel();
            model.Load();
            return View("Index", model);
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
    }
}