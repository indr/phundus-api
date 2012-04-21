namespace phiNdus.fundus.Business
{
    public interface IMailGateway
    {
        void Send(string recipients, string subject, string body);
    }
}