namespace Phundus.Core.SettingsCtx
{
    public interface IMailTemplatesSettings
    {
        IMailTemplateSettings OrderRejected { get; }
        IMailTemplateSettings OrderApproved { get; }
        IMailTemplateSettings OrderReceived { get; }
        IMailTemplateSettings UserAccountValidation { get; }
        IMailTemplateSettings UserAccountCreated { get; }
        IMailTemplateSettings UserChangeEmailValidationMail { get; }
        IMailTemplateSettings UserResetPasswordMail { get; }
        IMailTemplateSettings UserUnlocked { get; }
        IMailTemplateSettings UserLockedOut { get; }
    }
}