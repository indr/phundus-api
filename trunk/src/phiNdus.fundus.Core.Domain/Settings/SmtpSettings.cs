using System;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public class SmtpSettings : BaseSettings, ISmtpSettings
    {
        public SmtpSettings(string keyspace) : base(keyspace)
        {
            
        }

        public string Host
        {
            get { return GetString("host"); }
        }
    }
}