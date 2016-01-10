namespace Phundus.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Net.Mime;
    using Infrastructure.Gateways;
    using RazorEngine;

    public abstract class BaseMail
    {
        public const string TextSignature = @"

--
This is an automatically generated message from phundus.
-
If you think it was sent incorrectly contact the administrators at lukas.mueller@phundus.ch or reto.inderbitzin@phundus.ch.";

        public const string HtmlFooter = @"<hr />
<footer>
    <p>This is an automatically generated message from phundus.<br />If you think it was sent incorrectly contact the administrators at lukas.mueller@phundus.ch or reto.inderbitzin@phundus.ch.</p>
</footer>
</div>
</body>
</html>
";

        private const string HtmlHeader = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN""
    ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html;charset=utf-8"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://@Model.Urls.ServerUrl/Content/bootstrap.min.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://@Model.Urls.ServerUrl/Content/fundus.css"" />
    <style type=""text/css"">
    body { margin: 0; padding: 0; color: #333333; font-family: Helvetica Neue,Helvetica,Arial,sans-serif; font-size: 13px; }
    </style>
</head>
<body>
<div class=""container"" style=""margin: 10px; padding: 0;"">";

        private readonly IMailGateway _gateway;
        private IList<Attachment> _attachments = new List<Attachment>();
        private dynamic _model = new {};

        protected BaseMail(IMailGateway mailGateway)
        {
            if (mailGateway == null) throw new ArgumentNullException("mailGateway");
            _gateway = mailGateway;
        }

        public dynamic Model
        {
            get { return _model; }
            set { _model = value; }
        }

        protected IList<Attachment> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        private string GenerateSubject(string value)
        {
            var subject = value;
            if (String.IsNullOrWhiteSpace(subject))
                return String.Empty;
            return @"[phundus] " + Razor.Parse(subject, Model);
        }

        private string GenerateTextBody(string value)
        {
            var textBody = value;
            if (String.IsNullOrWhiteSpace(textBody))
                return String.Empty;
            return Razor.Parse(textBody + TextSignature, Model);
        }

        private string GenerateHtmlBody(string value)
        {
            var htmlBody = value;
            if (String.IsNullOrWhiteSpace(htmlBody))
                return String.Empty;
            return Razor.Parse(HtmlHeader + htmlBody + HtmlFooter, Model);
        }

        protected void Send(string recipients, string subject, string plain, string html)
        {
            if (String.IsNullOrWhiteSpace(recipients))
                return;


            var textBody = GenerateTextBody(plain);
            var htmlBody = GenerateHtmlBody(html);

            var message = new MailMessage {Subject = GenerateSubject(subject)};

            message.To.Add(recipients);

            if (!string.IsNullOrEmpty(htmlBody) && !string.IsNullOrEmpty(textBody))
            {
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody,
                    new ContentType(ContentTypes.Html)));
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textBody,
                    new ContentType(ContentTypes.Text)));
            }
            else if (!string.IsNullOrEmpty(htmlBody))
            {
                message.Body = htmlBody;
                message.IsBodyHtml = true;
            }
            else if (!string.IsNullOrEmpty(textBody))
            {
                message.Body = textBody;
                message.IsBodyHtml = false;
            }

            foreach (var each in Attachments)
                message.Attachments.Add(each);

            _gateway.Send(message);
        }

        public class Urls
        {
            private readonly string _serverUrl;

            public Urls(string serverUrl)
            {
                _serverUrl = serverUrl;
            }

            public string ServerUrl
            {
                get { return _serverUrl; }
            }

            public string UserAccountValidation
            {
                get { return "http://" + _serverUrl + "/#/validate/account"; }
            }

            public string UserEmailValidation
            {
                get { return "http://" + _serverUrl + "/#/validate/email-address"; }
            }
        }
    }

    public static class ContentTypes
    {
        public const string Html = "text/html";
        public const string Text = "text/plain";
    }
}