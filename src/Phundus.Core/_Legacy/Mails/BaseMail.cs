namespace Phundus.Core.Mails
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Net.Mime;
    using Microsoft.Practices.ServiceLocation;
    using Phundus.Infrastructure.Gateways;
    using Settings;
    using RazorEngine;

    public class BaseMail
    {
        protected BaseMail(IMailTemplateSettings settings) : this(settings.Subject, settings.TextBody, settings.HtmlBody)
        {
        }

        protected BaseMail(string subject, string textBody, string htmlBody)
        {
            Subject = subject;
            TextBody = textBody;
            HtmlBody = htmlBody;
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
                get { return "http://" + _serverUrl + "/account/validation"; }
            }

            public string UserEmailValidation
            {
                get { return "http://" + _serverUrl + "/account/emailvalidation"; }
            }
        }

        //protected IDictionary<string, object> DataContext
        //{
        //    get { return _dataContext; }
        //}

        public string Subject { get; protected set; }
        public string TextBody { get; protected set; }
        public string HtmlBody { get; protected set; }

        private dynamic _model = new {};
        public dynamic Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private const string TextSignature = @"

--
This is an automatically generated message from phundus.
-
If you think it was sent incorrectly contact the administrator(s) at @Model.Admins.";

        private const string HtmlFooter = @"<hr />
<footer>
    <p>This is an automatically generated message from phundus.<br />If you think it was sent incorrectly contact the administrator(s) at @Model.Admins.</p>
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

        private string GenerateSubject()
        {
            if (String.IsNullOrWhiteSpace(Subject))
                return String.Empty;
            return Razor.Parse(Subject, Model);
        }

        private string GenerateTextBody()
        {
            if (String.IsNullOrWhiteSpace(TextBody))
                return String.Empty;
            return Razor.Parse(TextBody + TextSignature, Model);
        }

        private string GenerateHtmlBody()
        {
            if (String.IsNullOrWhiteSpace(HtmlBody))
                return String.Empty;
            return Razor.Parse(HtmlHeader + HtmlBody + HtmlFooter, Model);
        }

        private IList<Attachment> _attachments = new List<Attachment>();
        protected IList<Attachment> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        protected void Send(string recipients)
        {
            var gateway = ServiceLocator.Current.GetInstance<IMailGateway>();

            var textBody = GenerateTextBody();
            var htmlBody = GenerateHtmlBody();

            var message = new MailMessage { Subject = GenerateSubject() };
            
            message.To.Add(recipients);
            
            if (!string.IsNullOrEmpty(htmlBody) && !string.IsNullOrEmpty(textBody))
            {
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, new ContentType(ContentTypes.Html)));
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textBody, new ContentType(ContentTypes.Text)));
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

            gateway.Send(message);
        }

        //private string GetValue(string key)
        //{
        //    int idx = key.LastIndexOf('.');
        //    if (idx == -1)
        //        return null;

        //    string group = key.Substring(0, idx);


        //    object data = null;
        //    if (!_dataContext.TryGetValue(group, out data))
        //        return null;

        //    var name = key.Substring(idx + 1);
        //    var propertyInfos = data.GetType().GetProperties();

        //    foreach (var each in propertyInfos.Where(each => each.Name == name))
        //    {
        //        var value = each.GetValue(data, null);
        //        return value == null ? "" : value.ToString();
        //    }
        //    return null;
        //}


        //private string ReplacePlaceholders(string input)
        //{
        //    return ReplacePlaceholders(input, 1);
        //}

        //private string ReplacePlaceholders(string input, int depth)
        //{
        //    string result = input;
        //    var regex = new Regex(@"\[([^\]]*)\]");
        //    Match match = regex.Match(input);
        //    while (match.Success)
        //    {
        //        string value = GetValue(match.Groups[1].Value);
        //        if (value != null)
        //            result = result.Replace(match.Value, value);
        //        match = match.NextMatch();
        //    }
        //    if (depth < 2)
        //        return ReplacePlaceholders(result, depth + 1);
        //    return result;
        //}
    }

    public static class ContentTypes
    {
        public const string Html = "text/html";
        public const string Text = "text/plain";
    }
}