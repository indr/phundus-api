﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Mails
{
    public abstract class AbstractMail
    {
        private readonly IDictionary<string, object> _dataContext = new Dictionary<string, object>();

        protected IDictionary<string, object> DataContext
        {
            get { return _dataContext; }
        }

        protected abstract string Subject { get; }
        protected abstract string Body { get; }

        private static string Signature
        {
            get
            {
                return
                    "\r\n\r\n--\r\nThis is automatically generated message from fundus.\r\n-\r\nIf you think it was sent incorrectly contact the administrator at admin@example.com.";
            }
        }

        protected void Send(string recipients)
        {
            var gateway = IoC.Resolve<IMailGateway>();
            gateway.Send(recipients, GenerateSubject(), GenerateBody());
        }

        private string GetValue(string key)
        {
            var idx = key.LastIndexOf('.');
            if (idx == -1)
                return null;

            var group = key.Substring(0, idx);


            object data = null;
            if (!_dataContext.TryGetValue(group, out data))
                return null;

            var name = key.Substring(idx + 1);
            var propertyInfos = data.GetType().GetProperties();

            foreach (var each in propertyInfos.Where(each => each.Name == name))
                return each.GetValue(data, null).ToString();
            return null;
        }


        private string ReplacePlaceholders(string input)
        {
            var result = input;
            var regex = new Regex(@"\[([^\]]*)\]");
            var match = regex.Match(input);
            while (match.Success)
            {
                var value = GetValue(match.Groups[1].Value);
                if (value != null)
                    result = result.Replace(match.Value, value);
                match = match.NextMatch();
            }
            return result;
        }

        private string GenerateSubject()
        {
            return ReplacePlaceholders(Subject);
        }

        private string GenerateBody()
        {
            return ReplacePlaceholders(Body + Signature);
        }
    }
}