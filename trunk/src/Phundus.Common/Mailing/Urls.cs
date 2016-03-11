namespace Phundus.Common.Mailing
{
    using System;

    public class Urls
    {
        private readonly string _baseUrl;

        public Urls() : this(Config.BaseUrl)
        {
        }

        public Urls(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
        }

        public string AccountValidation(string key = null)
        {
            return BaseUrl + "#/validate/account" + (String.IsNullOrWhiteSpace(key) ? "" : "?key=" + key);
        }

        public string EmailAddressValidation(string key = null)
        {
            return BaseUrl + "#/validate/email-address" + (String.IsNullOrWhiteSpace(key) ? "" : "?key=" + key);
        }

        public string Make(string resource)
        {

            return BaseUrl + resource.TrimStart('/');
        }
    }
}