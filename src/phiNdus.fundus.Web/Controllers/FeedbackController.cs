using System;
using System.Configuration;
using System.Web.Mvc;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Web.Models;
using Rhino.Commons;

namespace phiNdus.fundus.Web.Controllers
{
    public class FeedbackController : Controller
    {
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

            var mailGateway = IoC.Resolve<IMailGateway>();
            var body = "Feedback von " + model.EmailAddress + Environment.NewLine + Environment.NewLine + model.Comment;
            //mailGateway.Send("mail@indr.ch,lukas.mueller@piNuts.ch", "[fundus] Feedback", body);
            mailGateway.Send(ConfigurationManager.AppSettings["FeedbackRecipients"] + "," + model.EmailAddress, "[phundus] Feedback", body);
            return View("Done");
        }
    }
}