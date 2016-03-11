namespace Phundus.Common
{
    using System;
    using System.Configuration;

    public static class Config
    {
        static Config()
        {
            var settings = ConfigurationManager.AppSettings;

            FeedbackRecipients = settings["FeedbackRecipients"];
            InMaintenance = Convert.ToBoolean(settings["MaintenanceMode"]);
            ServerUrl = settings["ServerUrl"] ?? "";
        }

        public static readonly string FeedbackRecipients;
        public static bool InMaintenance { get; set; }
        public static readonly string ServerUrl;
    }
}