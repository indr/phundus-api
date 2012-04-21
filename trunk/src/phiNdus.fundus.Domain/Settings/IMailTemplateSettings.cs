namespace phiNdus.fundus.Core.Domain.Settings
{
    public interface IMailTemplateSettings
    {
        string Subject { get; }
        string Body { get; }
    }
}