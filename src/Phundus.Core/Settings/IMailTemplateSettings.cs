namespace Phundus.Core.Settings
{
    public interface IMailTemplateSettings
    {
        string Subject { get; }
        string TextBody { get; }
        string HtmlBody { get; }
    }
}