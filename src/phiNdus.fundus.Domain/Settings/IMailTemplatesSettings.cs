namespace phiNdus.fundus.Domain.Settings
{
    public interface IMailTemplatesSettings
    {
        
        IMailTemplateSettings OrderRejected { get; }
        IMailTemplateSettings OrderApproved { get; }
        IMailTemplateSettings OrderReceived { get; }
        IMailTemplateSettings UserAccountValidation { get; }
        IMailTemplateSettings UserAccountCreated { get; }
        IMailTemplateSettings UserChangeEmailValidationMail { get; }
        IMailTemplateSettings UserUnlocked { get; }
        IMailTemplateSettings UserLockedOut { get; }
        
    }
}