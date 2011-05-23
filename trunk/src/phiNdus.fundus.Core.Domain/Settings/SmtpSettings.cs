using System;

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

        public string From
        {
            get { return GetString("from"); }
        }

        public string UserName
        {
            get { return GetString("user-name"); }
        }

        public string Password
        {
            get { return GetString("password"); }
        }

        #endregion
    }
}