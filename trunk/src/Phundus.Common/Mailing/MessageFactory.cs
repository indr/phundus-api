namespace Phundus.Common.Mailing
{
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using RazorEngine;

    public interface IMessageFactory
    {
        MailMessage MakeMessage(dynamic data, string subject, string textBody, string htmlBody);
    }

    public class MessageFactory : IMessageFactory
    {
        private readonly IModelFactory _modelFactory;

        public MessageFactory(IModelFactory modelFactory)
        {
            if (modelFactory == null) throw new ArgumentNullException("modelFactory");
            _modelFactory = modelFactory;
        }

        public MailMessage MakeMessage(dynamic data, string subject, string textBody, string htmlBody)
        {
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException("subject");
            if (String.IsNullOrWhiteSpace(textBody) && String.IsNullOrWhiteSpace(htmlBody))
                throw new ArgumentNullException("textBody",
                    "You must provide either a text body template or a html text body template.");
            var hasTextAndHtmlBody = !String.IsNullOrWhiteSpace(textBody) && !String.IsNullOrWhiteSpace(htmlBody);
            var hasTextBody = !String.IsNullOrWhiteSpace(textBody);

            var model = _modelFactory.MakeModel(data);

            var result = new MailMessage();
            result.Subject = ParseSubject(model, subject);

            if (hasTextAndHtmlBody)
            {
                result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseHtmlBody(model, htmlBody),
                    new ContentType(ContentTypes.Html)));
                result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseTextBody(model, textBody),
                    new ContentType(ContentTypes.Text)));
            }
            else if (hasTextBody)
                result.Body = ParseTextBody(model, textBody);
            else
            {
                result.IsBodyHtml = true;
                result.Body = ParseHtmlBody(model, htmlBody);
            }

            return result;
        }

        private string ParseSubject(dynamic model, string subject)
        {
            return @"[phundus] " + Razor.Parse(subject, model);
        }

        private string ParseTextBody(dynamic model, string textBody)
        {
            return Razor.Parse(textBody + MailTemplates.TextSignature, model);
        }

        private string ParseHtmlBody(dynamic model, string htmlBody)
        {
            return Razor.Parse(MailTemplates.HtmlHeader + htmlBody + MailTemplates.HtmlFooter, model);
        }
    }
}