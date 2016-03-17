namespace Phundus.Rest.Api
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common;
    using Common.Mailing;
    using Common.Resources;
    using Newtonsoft.Json;

    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiControllerBase
    {
        private readonly IMailGateway _mailGateway;

        public FeedbackController(IMailGateway mailGateway)
        {
            _mailGateway = mailGateway;
        }

        [POST("")]
        [AllowAnonymous]
        public virtual HttpResponseMessage Post(FeedbackPostRequestContent rq)
        {
            if (rq == null) throw new ArgumentNullException("rq");

            _mailGateway.Send(DateTime.UtcNow, Config.FeedbackRecipients,
                @"[phundus] Feedback",
                @"Feedback von " + rq.EmailAddress + Environment.NewLine + Environment.NewLine +
                rq.Comment + MailTemplates.TextSignature);

            _mailGateway.Send(DateTime.UtcNow, rq.EmailAddress,
                @"Vielen Dank fürs Feedback",
                @"Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

Vielen Dank und freundliche Grüsse

Das phundus-Team" + MailTemplates.TextSignature);

            return NoContent();
        }
    }

    public class FeedbackPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}