using System;

namespace phiNdus.fundus.Domain.Settings
{
    public class CommonSettings : BaseSettings, ICommonSettings
    {
        public CommonSettings(string keyspace) : base(keyspace)
        {
        }

        public string ServerUrl
        {
            get { return GetString("server-url", "localhost"); }
        }

        public string AdminEmailAddress
        {
            get { return GetString("admin-email-address", ""); }
        }
    }
}