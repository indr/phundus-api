using System;

namespace phiNdus.fundus.Domain.Settings
{
    public static class Settings
    {
        [ThreadStatic] private static ISettings _settings;
        private static ISettings _globalNonThreadSafeSettings;

        public static IMailSettings Mail
        {
            get { return GetSettings().Mail; }
        }

        public static ICommonSettings Common
        {
            get { return GetSettings().Common; }
        }

        public static ISettings GetSettings()
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