namespace phiNdus.fundus.Domain.Settings
{
    public interface IMailTemplateSettings
    {
        string Subject { get; }
        string Body { get; }
    }
}