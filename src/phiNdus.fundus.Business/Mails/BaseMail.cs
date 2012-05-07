using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Domain.Settings;
using RazorEngine;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Mails
{
    public class BaseMail
    {
        //private readonly IDictionary<string, object> _dataContext = new Dictionary<string, object>();

        protected BaseMail(IMailTemplateSettings settings) : this(settings.Subject, settings.TextBody, settings.HtmlBody)
        {
            
        }

        protected BaseMail(string subject, string textBody, string htmlBody)
        {
            Subject = subject;
            TextBody = textBody;
            //_dataContext.Add("Link", new Urls(Settings.Common.ServerUrl));
        }

        public class Urls
        {
            private readonly string _serverUrl;

            public Urls(string serverUrl)
            {
                _serverUrl = serverUrl;
            }

            public string UserAccountValidation
            {
                get { return "http://" + _serverUrl + "/account/validation"; }
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
            return Razor.Parse(TextBody, Model);
        }

        private string GenerateHtmlBody()
        {
            if (String.IsNullOrWhiteSpace(HtmlBody))
                return String.Empty;
            return Razor.Parse(HtmlBody, Model);
        }

        protected void Send(string recipients)
        {
            var gateway = IoC.Resolve<IMailGateway>();

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