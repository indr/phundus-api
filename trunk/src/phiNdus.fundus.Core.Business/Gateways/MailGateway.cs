using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace phiNdus.fundus.Core.Business
{
    public class MailGateway : IMailGateway
    {
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


        private string _host;
        private string _userName;
        private string _password;
        private string _from;

        public void Send(string recipients, string subject, string body)
        {
            var client = new SmtpClient(_host);
            client.Credentials = new NetworkCredential(_userName, _password);
            client.Send(_from, recipients, subject, body);
        }
    }
}
