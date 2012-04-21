namespace phiNdus.fundus.Domain.Settings
{
    public interface IMailSettings
    {
        ISmtpSettings Smtp { get; }
        IMailTemplatesSettings Templates { get; }
    }
}