namespace piNuts.phundus.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Browsers;

    public class StepBase
    {
        protected static IeBrowser Browser
        {
            get { return Browsers.Browser.Current; }
        }

        protected static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["ServerUrl"]; }
        }

        protected static void Login(string username, string password)
        {
            ApiCall("/auth/login", new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password}
                });
        }

        protected static void ApiCall(string url)
        {
            ApiCall(url, new Dictionary<string, string>());
        }

        protected static void ApiCall(string url, IEnumerable<KeyValuePair<string, string>> formValues)
        {
            var content = new FormUrlEncodedContent(formValues);
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = Browser.GetCookieContainerForUrl(new Uri("http://" + BaseUrl));
            var client = new HttpClient(handler);
            var response = client.PostAsync("http://" + BaseUrl + "/api/" + url, content).Result;
            response.EnsureSuccessStatusCode();
            foreach (var each in cookies.GetCookies(new Uri("http://" + BaseUrl)).Cast<Cookie>())
            {
                var data = String.Format("{0}={1}; expires={2}", new object[]
                    {
                        each.Name,
                        each.Value,
                        DateTime.Now.AddMinutes(30).ToString("R")
                    });
                Browser.SetCookie("http://" + BaseUrl, data);
            }
        }
    }
}