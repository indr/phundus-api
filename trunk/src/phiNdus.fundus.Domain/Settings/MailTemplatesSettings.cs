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

        #endregion
    }
}