namespace phiNdus.fundus.Core.Domain.Settings
{
    public class SmtpSettings : BaseSettings, ISmtpSettings
    {
        public SmtpSettings(string keyspace) : base(keyspace)
        {
        }

        #region ISmtpSettings Members

        public string Host
        {
            get { return GetString("host"); }
        }

        #endregion
    }
}