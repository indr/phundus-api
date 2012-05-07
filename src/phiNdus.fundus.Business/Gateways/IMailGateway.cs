using System.Net.Mail;

namespace phiNdus.fundus.Business.Gateways
{
    public interface IMailGateway
    {
        void Send(string recipients, string subject, string body);
        void Send(MailMessage message);
    }
}