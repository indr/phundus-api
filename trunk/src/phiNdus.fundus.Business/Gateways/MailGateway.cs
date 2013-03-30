using System;
using System.Net;
using System.Net.Mail;
using phiNdus.fundus.Domain.Settings;

namespace phiNdus.fundus.Business.Gateways
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
            var message = new MailMessage();
            message.To.Add(recipients);
            message.Subject = subject;
            message.Body = body;
            Send(message);
        }

        public void Send(MailMessage message)
        {
            new SmtpClient().Send(message);
        }

        #endregion
    }
}