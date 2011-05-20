﻿using System;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public class MailTemplateSettings : BaseSettings, IMailTemplateSettings
    {
        public MailTemplateSettings(string keyspace) : base(keyspace)
        {
        }

        public string Subject
        {
            get { return GetString("subject"); }
        }

        public string Body
        {
            get { return GetString("body"); }
        }
    }
}