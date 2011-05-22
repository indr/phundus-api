using System.Net;
using System.Net.Mail;

namespace phiNdus.fundus.Core.Business
{
    public class MailGateway : IMailGateway
    {
        private readonly string _from;
        private readonly string _host;
        private readonly string _password;
        private readonly string _userName;

        public MailGateway()
        {
            // TODO: Credentials aus Config.
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