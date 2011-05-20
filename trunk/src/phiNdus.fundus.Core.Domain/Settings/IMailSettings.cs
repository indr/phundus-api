namespace phiNdus.fundus.Core.Domain.Settings
{
    public interface IMailSettings
    {
        ISmtpSettings Smtp { get; }
        IMailTemplatesSettings Templates { get; }
    }
}