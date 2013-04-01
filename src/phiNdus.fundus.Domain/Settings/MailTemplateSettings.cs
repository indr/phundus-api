namespace phiNdus.fundus.Domain.Settings
{
    public class MailTemplateSettings : BaseSettings, IMailTemplateSettings
    {
        public MailTemplateSettings(string keyspace) : base(keyspace)
        {
        }

        #region IMailTemplateSettings Members

        public string Subject
        {
            get { return GetString("subject"); }
        }

        public string TextBody
        {
            get { return GetString("body"); }
        }

        public string HtmlBody
        {
            get { return GetString("html-body"); }
        }

        #endregion
    }
}