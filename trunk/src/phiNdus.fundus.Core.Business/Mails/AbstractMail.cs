using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Mails
{
    public abstract class AbstractMail
    {
        protected Dictionary<string, object> DataContext = new Dictionary<string, object>();
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
            int idx = key.LastIndexOf('.');
            if (idx == -1)
                return null;

            string group = key.Substring(0, idx);


            object data = null;
            if (!DataContext.TryGetValue(group, out data))
                return null;

            string name = key.Substring(idx + 1);
            PropertyInfo[] propertyInfos = data.GetType().GetProperties();

            foreach (PropertyInfo each in propertyInfos.Where(each => each.Name == name))
                return each.GetValue(data, null).ToString();
            return null;
        }


        private string ReplacePlaceholders(string input)
        {
            string result = input;
            var regex = new Regex(@"\[([^\]]*)\]");
            Match match = regex.Match(input);
            while (match.Success)
            {
                string value = GetValue(match.Groups[1].Value);
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