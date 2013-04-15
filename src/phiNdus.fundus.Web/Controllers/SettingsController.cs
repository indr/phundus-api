using System.Web.Mvc;
using Castle.Transactions;
using phiNdus.fundus.Web.ViewModels;

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
        [Transaction]
        public virtual ActionResult Index()
        {
            return MailTemplates();
        }

        [HttpGet]
        [Transaction]
        public virtual ActionResult MailTemplates()
        {
            ModelState.Clear();
            var model = new MailTemplateSettingsViewModel();
            model.Load();
            return View(Views.MailTemplates, MasterView, model);
        }

        [HttpPost]
        [Transaction]
        public virtual ActionResult MailTemplates(MailTemplateSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                
                    model.SaveChanges();
                    
                
                return MailTemplates();
            }

            return View(Views.MailTemplates, MasterView, model);
        }

        #region Nested type: Views

        private static class Views
        {
            public static string MailTemplates
            {
                get { return @"MailTemplates"; }
            }
        }

        #endregion
    }
}