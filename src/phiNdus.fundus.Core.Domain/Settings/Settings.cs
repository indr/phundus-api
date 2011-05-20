﻿using System;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public static class Settings
    {
        [ThreadStatic] private static ISettings _settings;
        private static ISettings _globalNonThreadSafeSettings;

        public static IMailSettings Mail
        {
            get { return GetSettings().Mail; }
        }

        private static ISettings GetSettings()
        {
            if (_globalNonThreadSafeSettings != null)
                return _globalNonThreadSafeSettings;
            return _settings ?? (_settings = new SettingsImpl());
        }

        public static void SetGlobalNonThreadSafeSettings(ISettings settings)
        {
            _globalNonThreadSafeSettings = settings;
        }
    }
}