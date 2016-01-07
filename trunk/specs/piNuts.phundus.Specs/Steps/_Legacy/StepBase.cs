namespace Phundus.Specs.Steps
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
            get { return Phundus.Specs.Browsers.Browser.Current; }
        }

        protected static string BaseUrl
        {
            get
            {
                var result = ConfigurationManager.AppSettings["ServerUrl"].ToString();
                if (result.StartsWith("http"))
                    return result;
                return "http://" + result;
            }
        }        

        protected static void Login(string username, string password)
        {

            ApiCall("/sessions", new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password}
                });
        }
        
        private static CookieContainer _container = new CookieContainer();

        protected static void ApiCall(string url)
        {
            ApiCall(url, HttpMethod.Post);
        }

        protected static void ApiCall(string url, HttpMethod method)
        {
            ApiCall(url, method, new Dictionary<string, string>());
        }

        protected static void ApiCall(string url, IEnumerable<KeyValuePair<string, string>> formValues)
        {
            ApiCall(url, HttpMethod.Post,  formValues);
        }

        protected static void ApiCall(string url, HttpMethod method, IEnumerable<KeyValuePair<string, string>> formValues)
        {
            //var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = _container;
            var client = new HttpClient(handler);
            url = BaseUrl + "/api/" + url;

            HttpResponseMessage response;

            if (method == HttpMethod.Delete)
                response = client.DeleteAsync(url).Result;
            else
                response = client.PostAsync(url, new FormUrlEncodedContent(formValues)).Result;

            response.EnsureSuccessStatusCode();

            foreach (var each in _container.GetCookies(new Uri(BaseUrl)).Cast<Cookie>())
            {
                var data = String.Format("{0}={1}; expires={2}", new object[]
                    {
                        each.Name,
                        each.Value,
                        DateTime.Now.AddMinutes(30).ToString("R")
                    });
                Browser.SetCookie(BaseUrl, data);
            }

        }
    }
}