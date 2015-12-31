namespace Phundus.Infrastructure.Gateways
{
    using System;
    using System.Net.Mail;
    using System.Web;

    public class MailGateway : IMailGateway
    {
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
            var client = GetClient();
            client.Send(message);
        }

        private static SmtpClient GetClient()
        {
            var client = new SmtpClient();
            if (!String.IsNullOrWhiteSpace(client.PickupDirectoryLocation))
            {
                if (!System.IO.Path.IsPathRooted(client.PickupDirectoryLocation))
                {
                    var path = HttpContext.Current.Server.MapPath(client.PickupDirectoryLocation);
                    client.PickupDirectoryLocation = path;
                }
            }
            return client;
        }

        #endregion
    }
}