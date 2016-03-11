namespace Phundus.Common.Mailing
{
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using RazorEngine;

    public interface IMessageFactory
    {
    }

    public class MessageFactory : IMessageFactory
    {
        private readonly bool _hasTextAndHtmlBody;
        private readonly bool _hasTextBody;
        private readonly string _htmlBody;
        private readonly IModelFactory _modelFactory;
        private readonly string _subject;
        private readonly string _textBody;

        public MessageFactory(IModelFactory modelFactory, string subject, string textBody, string htmlBody)
        {
            if (modelFactory == null) throw new ArgumentNullException("modelFactory");
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException("subject");
            if (String.IsNullOrWhiteSpace(textBody) && String.IsNullOrWhiteSpace(htmlBody))
                throw new ArgumentNullException("textBody",
                    "You must provide either a text body template or a html text body template.");
            _modelFactory = modelFactory;
            _subject = subject;
            _textBody = textBody;
            _htmlBody = htmlBody;

            _hasTextAndHtmlBody = !String.IsNullOrWhiteSpace(_textBody) && !String.IsNullOrWhiteSpace(_htmlBody);
            _hasTextBody = !String.IsNullOrWhiteSpace(_textBody);
        }

        public MailMessage MakeMessage(dynamic data)
        {
            var model = _modelFactory.MakeModel(data);

            var result = new MailMessage();
            result.Subject = ParseSubject(model);

            if (_hasTextAndHtmlBody)
            {
                result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseHtmlBody(model),
                    new ContentType(ContentTypes.Html)));
                result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(ParseTextBody(model),
                    new ContentType(ContentTypes.Text)));
            }
            else if (_hasTextBody)
                result.Body = ParseTextBody(model);
            else
            {
                result.IsBodyHtml = true;
                result.Body = ParseHtmlBody(model);
            }

            return result;
        }

        private object CreateModel(object data)
        {
            return new
            {
                Urls = new
                {
                    ServerUrl = ""
                },
                Data = data
            };
        }

        private string ParseSubject(dynamic model)
        {
            return @"[phundus] " + Razor.Parse(_subject, model);
        }

        private string ParseTextBody(dynamic model)
        {
            return Razor.Parse(_textBody + MailTemplates.TextSignature, model);
        }

        private string ParseHtmlBody(dynamic model)
        {
            return Razor.Parse(MailTemplates.HtmlHeader + _htmlBody + MailTemplates.HtmlFooter, model);
        }
    }
}