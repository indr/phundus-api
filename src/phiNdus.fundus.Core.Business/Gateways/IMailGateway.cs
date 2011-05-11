namespace phiNdus.fundus.Core.Business
{
    public interface IMailGateway
    {
        void Send(string recipients, string subject, string body);
    }
}
