using System;
using System.Configuration;
using System.Web.Mvc;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Web.Models;

namespace phiNdus.fundus.Web.Controllers
{
    public class FeedbackController : Controller
    {
        public IMailGateway MailGateway { get; set; }

        public ActionResult Index()
        {
            var model = new FeedbackModel();
            if (User.Identity.IsAuthenticated)
                model.EmailAddress = User.Identity.Name;
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FeedbackModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            const string subject = "[phundus] Feedback";
            var body = "Feedback von " + model.EmailAddress + Environment.NewLine + Environment.NewLine + model.Comment;
            MailGateway.Send(ConfigurationManager.AppSettings["FeedbackRecipients"], subject, body);
            MailGateway.Send(model.EmailAddress, subject, body);

            return View("Done");
        }
    }
}