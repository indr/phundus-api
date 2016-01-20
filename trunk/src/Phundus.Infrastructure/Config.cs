namespace Phundus.Infrastructure
{
    using System;
    using System.Configuration;

    public static class Config
    {
        public static readonly string ServerUrl;
        public static readonly string FeedbackRecipients;


        static Config()
        {
            var settings = ConfigurationManager.AppSettings;
            ServerUrl = settings["ServerUrl"];
            FeedbackRecipients = settings["FeedbackRecipients"];
            InMaintenance = Convert.ToBoolean(settings["MaintenanceMode"]);
        }

        public static bool InMaintenance { get; set; }
    }
}