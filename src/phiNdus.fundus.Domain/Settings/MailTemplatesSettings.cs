using System;

namespace phiNdus.fundus.Domain.Settings
{
    public class MailTemplatesSettings : BaseSettings, IMailTemplatesSettings
    {
        public MailTemplatesSettings(string keyspace) : base(keyspace)
        {
        }

        #region IMailTemplatesSettings Members

        public IMailTemplateSettings UserAccountValidation
        {
            get { return new MailTemplateSettings(Keyspace + ".user-account-validation"); }
        }

        public IMailTemplateSettings UserAccountCreated
        {
            get { return new MailTemplateSettings(Keyspace + ".user-account-created"); }
        }

        public IMailTemplateSettings OrderRejected
        {
            get { return new MailTemplateSettings(Keyspace + ".order-rejected"); }
        }

        public IMailTemplateSettings OrderApproved
        {
            get { return new MailTemplateSettings(Keyspace + ".order-approved"); }
        }

        public IMailTemplateSettings OrderReceived
        {
            get { return new MailTemplateSettings(Keyspace + ".order-received"); }
        }

        public IMailTemplateSettings UserUnlocked
        {
            get { return new MailTemplateSettings(Keyspace + ".user-unlocked"); }
        }

        public IMailTemplateSettings UserLockedOut
        {
            get { return new MailTemplateSettings(Keyspace + ".user-locked-out"); }
        }

        #endregion
    }
}