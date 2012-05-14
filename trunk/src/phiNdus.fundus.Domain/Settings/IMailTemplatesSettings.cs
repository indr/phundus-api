namespace phiNdus.fundus.Domain.Settings
{
    public interface IMailTemplatesSettings
    {
        IMailTemplateSettings UserAccountValidation { get; }
        IMailTemplateSettings UserAccountCreated { get; }
        IMailTemplateSettings OrderRejected { get; }
        IMailTemplateSettings OrderApproved { get; }
        IMailTemplateSettings OrderReceived { get; }
        IMailTemplateSettings UserUnlocked { get; }
        IMailTemplateSettings UserLockedOut { get; }
    }
}