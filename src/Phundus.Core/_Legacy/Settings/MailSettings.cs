namespace Phundus.Core.Settings
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

        public IMailTemplatesSettings Templates
        {
            get { return new MailTemplatesSettings(Keyspace + ".templates"); }
        }

        #endregion
    }
}