using System;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public class MailTemplatesSettings : BaseSettings, IMailTemplatesSettings
    {
        public MailTemplatesSettings(string keyspace) : base(keyspace)
        {
        }

        public IMailTemplateSettings UserAccountValidation
        {
            get { return new MailTemplateSettings(Keyspace + ".user-account-validation"); }
        }
    }
}