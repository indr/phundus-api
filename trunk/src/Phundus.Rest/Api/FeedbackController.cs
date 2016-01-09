namespace Phundus.Rest.Api
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Core;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Newtonsoft.Json;

    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiControllerBase
    {
        private readonly IMailGateway _mailGateway;

        public FeedbackController(IMailGateway mailGateway)
        {
            if (mailGateway == null) throw new ArgumentNullException("mailGateway");
            _mailGateway = mailGateway;
        }

        [POST("")]
        [AllowAnonymous]
        public virtual HttpResponseMessage Post(FeedbackPostRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            _mailGateway.Send(Config.FeedbackRecipients,
               @"[phundus] Feedback",
               @"Feedback von " + requestContent.EmailAddress + Environment.NewLine + Environment.NewLine +
               requestContent.Comment + BaseMail.TextSignature);

            _mailGateway.Send(requestContent.EmailAddress,
               @"Vielen Dank fürs Feedback",
               @"Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

Vielen Dank und freundliche Grüsse

Das phundus-Team" + BaseMail.TextSignature);

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