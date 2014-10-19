namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core;
    using Infrastructure;
    using Infrastructure.Gateways;
    using phiNdus.fundus.Web.Models;

    public class FeedbackController : ControllerBase
    {
        public IMailGateway MailGateway { get; set; }

        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            var model = new FeedbackModel();
            if (Identity.IsAuthenticated)
                model.EmailAddress = Identity.Name;
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Transaction]
        [AllowAnonymous]
        public virtual ActionResult Index(FeedbackModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            MailGateway.Send(Config.FeedbackRecipients,
                @"[phundus] Feedback",
                @"Feedback von " + model.EmailAddress + Environment.NewLine + Environment.NewLine +
                model.Comment + BaseMail.TextSignature);


            MailGateway.Send(model.EmailAddress,
                @"Vielen Dank fürs Feedback",
                @"Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

Vielen Dank und freundliche Grüsse

Das phundus-Team" + BaseMail.TextSignature);

            return View("Done");
        }
    }
}