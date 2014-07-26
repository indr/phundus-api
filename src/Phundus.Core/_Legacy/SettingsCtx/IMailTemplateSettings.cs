namespace Phundus.Core.SettingsCtx
{
    public interface IMailTemplateSettings
    {
        string Subject { get; }
        string TextBody { get; }
        string HtmlBody { get; }
    }
}