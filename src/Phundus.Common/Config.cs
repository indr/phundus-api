namespace Phundus.Common
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class Config
    {       
// ReSharper disable once InconsistentNaming
        private static readonly ConfigImpl _config;

        static Config()
        {
            _config = new ConfigImpl(ConfigurationManager.AppSettings);
        }

        public static string BaseUrl
        {
            get { return _config.BaseUrl; }
        }

        public static string FeedbackRecipients
        {
            get { return _config.FeedbackRecipients; }
        }

        public static bool InMaintenance
        {
            get { return _config.InMaintenance; }
            set { _config.InMaintenance = value; }
        }

        public static string ServerUrl
        {
            get { return _config.ServerUrl; }
        }
    }

    public class ConfigImpl
    {
        private string _baseUrl;
        private string _serverUrl;

        public ConfigImpl(NameValueCollection settings)
        {
            FeedbackRecipients = settings["FeedbackRecipients"];
            InMaintenance = Convert.ToBoolean(settings["MaintenanceMode"]);
            ServerUrl = settings["ServerUrl"] ?? "";
            BaseUrl = ServerUrl;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            private set
            {
                if (value == "www.phundus.ch")
                {
                    _baseUrl = "https://www.phundus.ch/";
                    return;
                }

                _baseUrl = "http://" + value + "/";
            }
        }

        public string ServerUrl
        {
            get { return _serverUrl; }
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                    value = "localhost";
                else if (value == "phundus.ch")
                    value = "www.phundus.ch";
                _serverUrl = value;
            }
        }

        public bool InMaintenance { get; set; }

        public string FeedbackRecipients { get; private set; }
    }
}