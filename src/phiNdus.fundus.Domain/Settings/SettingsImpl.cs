﻿namespace phiNdus.fundus.Domain.Settings
{
    public class SettingsImpl : ISettings
    {
        private IMailSettings _mail;

        #region ISettings Members

        public IMailSettings Mail
        {
            get { return _mail ?? (_mail = new MailSettings("mail")); }
        }

        #endregion
    }
}