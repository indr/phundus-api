using System;
using System.Configuration;
using System.Web.Mvc;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Web.Models;

namespace phiNdus.fundus.Web.Controllers
{
    using Domain.Infrastructure;

    public class FeedbackController : ControllerBase
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

            MailGateway.Send(Config.FeedbackRecipients,
                @"[phundus] Feedback",
                @"Feedback von " + model.EmailAddress + Environment.NewLine + Environment.NewLine + model.Comment);
            
            
            MailGateway.Send(model.EmailAddress,
                @"Vielen Dank fürs Feedback",
                @"Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

Vielen Dank und freundliche Grüsse

Das phundus-Team");

            return View("Done");
        }
    }
}