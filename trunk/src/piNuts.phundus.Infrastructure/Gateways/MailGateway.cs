namespace Phundus.Infrastructure.Gateways
{
    using System.Net.Mail;

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
            //new SmtpClient().Send(message);
        }

        #endregion
    }
}