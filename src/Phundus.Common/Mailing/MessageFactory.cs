namespace Phundus.Common.Mailing
{
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using RazorEngine;
    using RazorEngine.Templating;

    public interface IMessageFactory
    {
        MailMessage MakeMessage(object model, string subject, string textBody, string htmlBody);
    }

    public class MessageFactory : IMessageFactory
    {
        public MailMessage MakeMessage(object model, string subject, string textBody, string htmlBody)
        {
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException("subject");
            if (String.IsNullOrWhiteSpace(textBody) && String.IsNullOrWhiteSpace(htmlBody))
                throw new ArgumentNullException("textBody",
                    "You must provide either a text body template or a html text body template.");
            var hasTextAndHtmlBody = !String.IsNullOrWhiteSpace(textBody) && !String.IsNullOrWhiteSpace(htmlBody);
            var hasTextBody = !String.IsNullOrWhiteSpace(textBody);

            var result = new MailMessage();
            result.Subject = ParseSubject(model, subject);

            if (hasTextAndHtmlBody)
            {
                result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseHtmlBody(model, htmlBody, result.Subject),
                    new ContentType(ContentTypes.Html)));
                result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseTextBody(model, textBody),
                    new ContentType(ContentTypes.Text)));
            }
            else if (hasTextBody)
                result.Body = ParseTextBody(model, textBody);
            else
            {
                result.IsBodyHtml = true;
                result.Body = ParseHtmlBody(model, htmlBody, result.Subject);
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

        private string ParseHtmlBody(dynamic model, string htmlBody, string subject)
        {
            var bag = new DynamicViewBag();
            bag.AddValue("Subject", subject);
            return Razor.Parse(MailTemplates.HtmlHeader + htmlBody + MailTemplates.HtmlFooter, model, bag, null);
        }
    }
}