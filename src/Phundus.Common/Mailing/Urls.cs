namespace Phundus.Common.Mailing
{
    public class Urls
    {
        private readonly string _serverUrl;

        public Urls() : this(Config.ServerUrl)
        {
        }

        public Urls(string serverUrl)
        {
            if (serverUrl == "phundus.ch")
                serverUrl = @"www." + serverUrl;
            var scheme = (serverUrl == @"www.phundus.ch" ? "https" : "http") + "://";

            if (!serverUrl.EndsWith("/"))
                serverUrl = serverUrl + "/";
            _serverUrl = scheme + serverUrl;
        }

        public string ServerUrl
        {
            get { return _serverUrl; }
        }

        public string UserAccountValidation
        {
            get { return ServerUrl + "#/validate/account"; }
        }

        public string UserEmailValidation
        {
            get { return ServerUrl + "#/validate/email-address"; }
        }
    }
}