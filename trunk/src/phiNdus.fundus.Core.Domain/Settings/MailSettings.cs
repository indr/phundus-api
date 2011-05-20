using System;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public class MailSettings : BaseSettings, IMailSettings
    {
        public MailSettings(string keyspace) : base(keyspace)
        {
        }

        public IMailTemplatesSettings TemplatesSettings
        {
            get { return new MailTemplatesSettings(Keyspace + ".templates"); }
        }

        #region IMailSettings Members

        public ISmtpSettings Smtp
        {
            get { return new SmtpSettings(Keyspace + ".smtp"); }
        }

        public IMailTemplatesSettings Templates
        {
            get { return new MailTemplatesSettings(Keyspace + ".templates"); }
        }

        #endregion
    }
}