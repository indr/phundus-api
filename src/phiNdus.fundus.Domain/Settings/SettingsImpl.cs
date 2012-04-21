using System;

namespace phiNdus.fundus.Domain.Settings
{
    public class SettingsImpl : ISettings
    {
        private IMailSettings _mail;
        private ICommonSettings _common;

        #region ISettings Members

        public IMailSettings Mail
        {
            get { return _mail ?? (_mail = new MailSettings("mail")); }
        }

        public ICommonSettings Common
        {
            get { return _common ?? (_common = new CommonSettings("common")); }
        }

        #endregion
    }
}