using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using phiNdus.fundus.Domain.Settings;
using RazorEngine;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Mails
{
    public class BaseMail
    {
        //private readonly IDictionary<string, object> _dataContext = new Dictionary<string, object>();

        protected BaseMail(IMailTemplateSettings settings) : this(settings.Subject, settings.Body)
        {
            
        }

        protected BaseMail(string subject, string body)
        {
            Subject = subject;
            Body = body;
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
        public string Body { get; protected set; }

        private dynamic _model = new {};
        public dynamic Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private static string Signature
        {
            get
            {
                return
                    @"

--
This is automatically generated message from fundus.
-
If you think it was sent incorrectly contact the administrator at @Model.Settings.Common.AdminEmail.";
            }
        }

        private string GenerateSubject()
        {
            return Razor.Parse(Subject, Model);
            //return ReplacePlaceholders(Subject);
        }

        private string GenerateBody()
        {
            return Razor.Parse(Body + Signature, Model);
            //return ReplacePlaceholders(Body + Signature);
        }

        protected void Send(string recipients)
        {
            var gateway = IoC.Resolve<IMailGateway>();
            
            gateway.Send(recipients, GenerateSubject(), GenerateBody());
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
}