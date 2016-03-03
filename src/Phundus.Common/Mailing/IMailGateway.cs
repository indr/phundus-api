namespace Phundus.Common.Mailing
{
    using System.Net.Mail;

    public interface IMailGateway
    {
        void Send(string recipients, string subject, string body);
        void Send(MailMessage message);
    }
}