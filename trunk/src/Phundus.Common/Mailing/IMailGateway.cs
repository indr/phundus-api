namespace Phundus.Common.Mailing
{
    using System;
    using System.Net.Mail;

    public interface IMailGateway
    {
        void Send(DateTime date, string recipients, string subject, string body);
        void Send(DateTime date, MailMessage message);
    }
}