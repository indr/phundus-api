using System.Net;
using System.Net.Mail;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Business.Gateways
{
    public class MailGateway : IMailGateway
    {
        private readonly string _from;
        private readonly string _host;
        private readonly string _password;
        private readonly string _userName;

        public MailGateway()
        {
            var settings = Settings.Mail.Smtp;
            _from = settings.From;
            _host = settings.Host;
            _password = settings.Password;
            _userName = settings.UserName;
        }

        public MailGateway(string host, string userName, string password, string from)
        {
            _host = host;
            _userName = userName;
            _password = password;
            _from = from;
        }

        #region IMailGateway Members

        public void Send(string recipients, string subject, string body)
        {
            var client = new SmtpClient(_host);
            client.Credentials = new NetworkCredential(_userName, _password);
            client.Send(_from, recipients, subject, body);
        }

        #endregion
    }
}